using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    /**
     * <summary> The model of Actor. </summary>
     */
    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 1.35f;
    public float runMultiplier = 2.7f;

    [SerializeField]            // 让变量可见，该变量必须被编译器支持
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 movingVec;      // move direction vector for fixedUpdate

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();           // get by itself
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        //if(rigid == null)
        //{

        //}
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
        if(pi.Dvec.magnitude > 0.01f)
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.1f);   // lerp as a vector not a point
            anim.transform.forward = targetForward;
            movingVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);
        }
    }

    private void FixedUpdate()              // Physics engine simulates 50 times pre second, it has a different speed fron update
    {
        // rigid.position += movingVec * Time.fixedDeltaTime;    // different update, must fix deltaTime
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z);
    }
    void playerMove()
    {

    }
}
