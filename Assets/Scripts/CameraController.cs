using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private IUserInput pi;
    public float minVertical = -80;
    public float maxVertical = 90;

    private float rotationX;
    private float rotationY;
    //private GameObject playerHandle;
    //private GameObject cameraHandle;

    [Header("Camera Setting")]
    [SerializeField]
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 1.0f;
    public bool isCameraCatch = false;
    public float cameraCatchTime = 0.2f;
    private Vector3 cameraDampVelocity;
    private Camera mainCamera;
    private GameObject cameraPos;

    [Header("Statics Rune Setting")]
    [SerializeField]
    private GameObject staticedObject;
    public Material staticMat;
    [SerializeField]
    private MeshRenderer[] targetMeshs;
    private List<Material> preMats = new List<Material>();
    private Material preStasisMat;

    private GameObject stasisedObject;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //cameraHandle = transform.parent.gameObject;
        transform.localEulerAngles = Vector3.zero;
        //playerHandle = cameraHandle.transform.parent.gameObject;
        mainCamera = Camera.main;
        Transform[] transforms = GetComponentsInChildren<Transform>();

        cameraPos = transforms[1].gameObject;
        pi = transform.parent.GetComponent<ActorController>().pi;
        //print(cameraPos.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        /**
         * <sumary> camera move and rotate </sumary>
         */
        rotationY = transform.localEulerAngles.y + pi.viewDRight * horizontalSpeed;
        rotationX = transform.localEulerAngles.x + pi.viewDUp * verticalSpeed;
        
        if(rotationX > 270)
        {
            rotationX -= 360;
        }
        rotationX = Mathf.Clamp(rotationX, minVertical, maxVertical);
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

        mainCamera.transform.position = isCameraCatch ? Vector3.SmoothDamp(mainCamera.transform.position, cameraPos.transform.position, ref cameraDampVelocity, cameraCatchTime) : cameraPos.transform.position;
        //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.transform.position, cameraMoveSpeed);
        //mainCamera.transform.rotation = cameraPos.transform.rotation;
        mainCamera.transform.LookAt(transform);
    }

    public void StaticRuneCamera()
    {
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView,50,0.5f);
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit,100,LayerMask.GetMask("ObjectSE")))
        {
            if(staticedObject == null)
            {
                staticedObject = hit.collider.gameObject;
                if(staticedObject != null)
                {
                    //print(hit.collider.transform.parent);
                    StasisObject tmp = staticedObject.GetComponent<StasisObject>();
                    //tmp.beStasisWatch = true;
                    if(tmp != null)
                    {
                        preStasisMat = tmp.stasisMat;
                        tmp.stasisMat = staticMat;
                    }
                    
                    //targetMeshs = staticedObject.GetComponentsInChildren<MeshRenderer>();
                    //for(int i = 0; i < targetMeshs.Length; i++)
                    //{
                    //    preMats.Add(targetMeshs[i].material);
                    //    targetMeshs[i].material = staticMat;
                    //}
                }
            }
            if(staticedObject!=null)
            {
                if(pi.lockRune && (pi.playerStasisEnable||staticedObject == stasisedObject))
                {
                    StasisObject tmp = staticedObject.GetComponent<StasisObject>();
                    if(tmp!=null)
                    {
                        tmp.ChangeStasisState();
                        if (tmp.beStasised)
                        {
                            stasisedObject = staticedObject; 
                        }
                    }
                }
            }
        }
        else
        {
            if(staticedObject != null)
            {
                StasisObject tmp = staticedObject.GetComponent<StasisObject>();
                //tmp.beStasisWatch = false;
                if(tmp!=null)
                {
                    tmp.stasisMat = preStasisMat;
                }
                    
                //for(int i = 0; i < targetMeshs.Length; i++)
                //{
                //    targetMeshs[i].material = preMats[i];
                //}

            }
            staticedObject = null;
            preMats.Clear();
        }
    }

    public void ResetRuneCamera()
    {
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, 0.5f);
        if(preMats.Count > 0 && staticedObject != null)
        {
            for (int i = 0; i < targetMeshs.Length; i++)
            {
                targetMeshs[i].material = preMats[i];
            }
            staticedObject = null;
            preMats.Clear();
        }
    }
}
