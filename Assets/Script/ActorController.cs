using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    /**
     * <summary> The model of Actor. </summary>
     */
    public GameObject model;
    public IUserInput pi;
    public float walkSpeed = 1.35f;
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 5.0f;
    public float rollLimiteSpeed = 10.0f;

    [Space(10)]
    [Header("Friction Settings")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;
    private CapsuleCollider playerCap;

    [SerializeField]            // 让变量可见，该变量必须被编译器支持
    private Animator anim;
    private int attackLayerIndex;
    private Rigidbody rigid;
    [SerializeField]
    private float avatarLerpTarget;
    /**
     * <summary>player planar velocity</summary>
     */
    private Vector3 planarVec;      // move direction vector for fixedUpdate
    /**
     * <summary>player velocity which be added by power,like jump, attack(</summary>
     */
    private Vector3 thrustVec;
    private bool lockPlanar = false;
    private bool attackAble = true;
    // for animator motion
    private Vector3 deltaPos;

    [Header("Debug Setting")]
    public GameObject deBugBall;
    private Vector3 playerLastPoint;
    private GameObject debugTmp;

    private void Awake()
    {
        IUserInput[] userInputs;
        userInputs = GetComponents<IUserInput>();
        foreach(var input in userInputs)
        {
            if(input.enabled == true)
            {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        //if(rigid == null)
        //{

        //}

        playerCap = GetComponent<CapsuleCollider>();
        attackLayerIndex = anim.GetLayerIndex("Attack Layer");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()                               // update for scene to display 60 times pre second
    {
        float targetRunMulti = pi.Dmag * (pi.run ? 2.0f : 1.0f);
        anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.3f) );
        /**
         * <sumary> roll </sumary>
         */
        if(rigid.velocity.magnitude > rollLimiteSpeed)
        {
            anim.SetTrigger("roll");
        }
        
        /**
         * <summary> jump </summary>
         */
        if (pi.isJump)
        {
            anim.SetTrigger("jump");
            attackAble = false;
        }

        // is player attack and onGround, attackAble
        // it is player can attack when jumping or other something.
        if(pi.isAttack && CheckState("groundBlend") && attackAble)
        {
            anim.SetTrigger("attack");
        }    

        // if move change the forward
        if(pi.Dvec.magnitude > 0.01f)
        {
            // slow trun forward
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.1f);   // lerp as a vector not a point
            anim.transform.forward = targetForward;

            // dont lock the planar vec
            if(!lockPlanar)
            {
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);
            }
        }
    }

    private void FixedUpdate()              // Physics engine simulates 50 times pre second, it has a different speed fron update
    {
        // rigid.position += planarVec * Time.fixedDeltaTime;    // different update, must fix deltaTime
        rigid.position += deltaPos;
        deltaPos = Vector3.zero;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }
    void playerMove()
    {

    }

    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    /// <summary>
    /// for jump enter
    /// </summary>

    public void OnJumpEnter()
    {
        thrustVec = new Vector3(0.0f, jumpVelocity, 0.0f);
    }

    //public void OnJumpExit()
    //{
    //    pi.inputEnable = true;
    //    lockPlanar = false;
    //}

    /**
     * be called in OnGroundSensor.
     */
    public void IsGround()
    {
        if(debugTmp != null)
        {
            Destroy(debugTmp);
        }
        anim.SetBool("isGround", true);
        playerLastPoint = this.transform.position;
    }
    /**
     * be called in OnGroundSensor.
     */
    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
        if(debugTmp == null)
        {
            debugTmp = GameObject.Instantiate(deBugBall, playerLastPoint+new Vector3(0,0.05f,0), Quaternion.identity);
        }
    }

    /**
     * be called in animator
     */
    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        lockPlanar = false;
        attackAble = true;
        playerCap.material = frictionOne;
    }

    /**
     * be called in animator
     */
    public void OnGroundExit()
    {
        playerCap.material = frictionZero;
    }

    // when enter attack1ha
    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
        // parameter index, parameter weight
        avatarLerpTarget = 1.0f;
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1.0f);
    }

    // when on attack1hA
    public void OnAttack1hAUpdate()
    {
        // NOT ON THE GROUND
        if(!anim.GetBool("isGround"))
        {
            // if in the sky, then attack down
            planarVec = Vector3.zero;
            thrustVec = -model.transform.up * 1.0f;
            
        }
        // ON THE GROUND
        else
        {
            planarVec = Vector3.Slerp(planarVec, Vector3.zero, 0.1f);
            // when attack give a velocity to forward
            thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
            
            float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer"));
            currentWeight = Mathf.Lerp(currentWeight, avatarLerpTarget, 0.05f);
            anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), currentWeight);
        }
    }

    public void OnAttackIdleEnter()
    {
        pi.inputEnable = true;
        lockPlanar = false;
        avatarLerpTarget = 0;
        
        //float currentWeight = anim.GetLayerWeight(attackLayerIndex);
        //currentWeight = Mathf.Lerp(currentWeight, avatarLerpTarget, 0.5f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), currentWeight);
    }

    public void OnAttackIdleUpdate()
    {
        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer"));
        currentWeight = Mathf.Lerp(currentWeight, avatarLerpTarget, 0.05f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), currentWeight);
    }

    public void OnUpdateRootMotion(object rmDeltPos)
    {
        // print("OnRootMotion!:" + deltPos);
        if (CheckState("attack1hC", "Attack Layer"))
        {
            print("Attack3 Motion!");
            deltaPos = (deltaPos + (Vector3)rmDeltPos) * 0.5f;
        }
    }
}
