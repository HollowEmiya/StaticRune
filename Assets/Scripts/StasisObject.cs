using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * if we want check some obejects is on the camera or not
 * can use about Render,suck as :
 * OnWillRenderObject¡¢ Renderer.isVisible¡¢ Renderer.OnBecameVisibleºÍ OnBecameInvisible
 * or£¬compute object's bounding box is on camera view£º
 * GeometryUtility.CalculateFrustumPlanes GeometryUtility.TestPlanesAABB
 */


public class StasisObject : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private IUserInput pi;
    public bool stasis;

    [Header("Object State")]
    public bool beStasised;
    public bool beStasisWatch;
    public float lockEnableTime = 15.0f;
    private float restLockTime;

    [Header("Object Materials")]
    public Material originMat;
    public Material stasisMat;
    public Material beStasisedMat;
    private MeshRenderer meshRenderer;

    [Header("Rigidbody Setting")]
    public Rigidbody rb;
    public Vector3 force;
    public float momentumStrength = 100.0f;

    [Header("Other")]
    [SerializeField]
    private Transform arrow;

    [Header("Particles")]
    public Transform startParticleGroup;
    public Transform endParticleGroup;

    private void Awake()
    {
        pi = GameObject.Find("PlayerHandle").GetComponent<IUserInput>();
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        arrow = transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(beStasised)
        {
            restLockTime -= Time.deltaTime;
            if(restLockTime < 0.01f)
            {
                ChangeStasisState();
            }
        }
        if (pi.CheckStasis() && !beStasised)
        {
            meshRenderer.material = stasisMat;
        }
        else if (pi.CheckStasis() && beStasised)
        {
            meshRenderer.material = beStasisedMat;
        }
        else if(!pi.CheckStasis()&& !beStasised)
        {
            meshRenderer.material = originMat;
        }
    }

    private void OnBecameVisible()
    {
        print(pi.CheckStasis());
        if(pi.CheckStasis() && !beStasised)
        {
            meshRenderer.material = stasisMat;
        }
        else if(pi.CheckStasis() && beStasised)
        {
            meshRenderer.material = beStasisedMat;
        }
    }

    public void ChangeStasisState()
    {
        print("Change!!!");
        beStasised = !beStasised;
        rb.isKinematic = beStasised;
        if (beStasised)
        {
            pi.playerStasisEnable = false;
            restLockTime = lockEnableTime;
            // arrow.gameObject.SetActive(true);
        }
        else if(!beStasised)
        {
            pi.playerStasisEnable = true;
            restLockTime = 0.0f;
            rb.AddForce(force);
            force = Vector3.zero;
            arrow.gameObject.SetActive(false);
        }
    }

    public void AddToForce(Vector3 hitPos, float forUp)
    {
        Vector3 dir = transform.position - hitPos;
        force = dir * (force.magnitude + momentumStrength * forUp);
        arrow.gameObject.SetActive(true);
        float arrowScale = Mathf.Min(arrow.localScale.z + 0.3f, 1.8f)*10;
        Vector3 tmp = new Vector3(arrow.localScale.x,arrow.localScale.y,arrowScale);
        arrow.localScale = tmp;

        arrow.rotation = Quaternion.LookRotation(dir);
    }

    public Vector3 computeDir(Vector3 hitPos)
    {
        return transform.position - hitPos;
    }
}
