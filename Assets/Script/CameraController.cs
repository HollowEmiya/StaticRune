using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public PlayerInput pi;
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

    private void Awake()
    {
        //cameraHandle = transform.parent.gameObject;
        transform.localEulerAngles = Vector3.zero;
        //playerHandle = cameraHandle.transform.parent.gameObject;
        mainCamera = Camera.main;
        Transform[] transforms = GetComponentsInChildren<Transform>();

        cameraPos = transforms[1].gameObject;

        print(cameraPos.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        mainCamera.transform.rotation = cameraPos.transform.rotation;
        /**
         * over
         */
    }
}
