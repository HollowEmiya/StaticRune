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


public class StasisObject1Mat : MonoBehaviour
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

    [Header("Object Materials Setting")]
    [SerializeField]
    private MeshRenderer meshRenderer;
    // public Material meshMat;
    [SerializeField]
    private Renderer objRenderer;
    public Color stasisEnableColor;
    public Color normalColor;
    public Color finalColor;

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
        objRenderer = GetComponent<Renderer>();
        // meshMat = meshRenderer.material;
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

        if (pi.CheckStasis() && !beStasised && !beStasisWatch)
        {
            objRenderer.material.SetColor("_Emission_Color", normalColor);

            objRenderer.material.SetFloat("_Stasis_Amount", 0.5f);
        }
        else if(pi.CheckStasis() && !beStasised && beStasisWatch)
        {
            objectBeStasisWatch();
        }
        //else if (pi.CheckStasis() && beStasised)
        //{
        //    objRenderer.material.SetColor("_Emission_Color", normalColor);
        //    objRenderer.material.SetFloat("_Stasis_Amount", 0.5f);
        //}
        
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
            objRenderer.material.SetFloat("_Stasis_Amount", 0.5f);
            objRenderer.material.SetFloat("_Noise_Amount", 1.0f);
            objRenderer.material.SetColor("_Emission_Color",normalColor);
        }
        else if(!beStasised)
        {
            pi.playerStasisEnable = true;
            restLockTime = 0.0f;
            rb.AddForce(force);
            force = Vector3.zero;
            arrow.gameObject.SetActive(false);
            if(!pi.CheckStasis())
            {
                objRenderer.material.SetFloat("_Stasis_Amount", 0.0f);

                objRenderer.material.SetColor("_Emission_Color", normalColor);
            }
            
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

        Transform arrowTr = arrow.gameObject.GetComponentsInChildren<Transform>()[1];
        Renderer arrowRender = arrowTr.gameObject.GetComponent<Renderer>();
        Color arrowColor = Color.Lerp(arrowRender.material.GetColor("_Color"), finalColor, 0.6f);
        arrowRender.material.SetColor("_Color",arrowColor);
        //print(arrowRender.material.name);

        //transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", arrowColor);
        //arrow.gameObject.GetComponentInChildren<Material>().SetColor("_Color", arrowColor);

        // set arrow dir
        arrow.rotation = Quaternion.LookRotation(dir);
        Color c = objRenderer.material.GetColor("_Emission_Color");
        c = Color.Lerp(c, finalColor, 0.3f);
        objRenderer.material.SetColor("_Emission_Color", c);
    }

    public Vector3 computeDir(Vector3 hitPos)
    {
        return transform.position - hitPos;
    }

    public void objectBeStasisWatch()
    {
        objRenderer.material.SetColor("_Emission_Color", stasisEnableColor);
        objRenderer.material.SetFloat("Stasis_Amount", 0.5f);
    }
}
