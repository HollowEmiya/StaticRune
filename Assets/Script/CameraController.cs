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
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 1.0f;
    private void Awake()
    {
        //cameraHandle = transform.parent.gameObject;
        transform.localEulerAngles = Vector3.zero;
        //playerHandle = cameraHandle.transform.parent.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationY = transform.localEulerAngles.y + pi.viewDRight * horizontalSpeed;
        rotationX = transform.localEulerAngles.x + pi.viewDUp * verticalSpeed;
        print("rotationX: " + rotationX);
        if(rotationX > 270)
        {
            rotationX -= 360;
        }
        rotationX = Mathf.Clamp(rotationX, minVertical, maxVertical);
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
}
