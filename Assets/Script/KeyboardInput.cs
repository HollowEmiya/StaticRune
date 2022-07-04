using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    // sometime we need change input-key but input-manager can't
    // we need the input defined by ourself
    [Header("===== Key Settings =====")]
    public string keyUp = "w";            
    public string keyDown = "s";            
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyViewRight;
    public string keyViewUp;

    //[Header("===== Output Siganl =====")]
    //public float Dup;
    //public float Dright;
    //public float Dmag;
    ///**
    // * <summary> move direction </summary>
    // */
    //public Vector3 Dvec;
    //public float viewDUp;
    //public float viewDRight;


    //// 1.pressing signal
    //public bool run;
    //// 2. trigger once type siganl
    //public bool isJump;
    //public bool isAttack;
    //// private bool lastJump;           // for simulate Input.GetKeyDown()
    //// 3. double trigger

    //[Header("===== Others =====")]
    //public bool inputEnable = true;
    //public GameObject cameraHandle;
    //public bool mouseIsVisible = false;

    //private float targetDup;
    //private float targetDright;
    //private float velocityDup;
    //private float velocityDright;

    private void Awake()
    {
        Cursor.visible = mouseIsVisible;

    }

    // Start is called before the first frame update
    void Start()
    {
        cameraHandle = GetComponentInChildren<CameraController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
         /*
        Dup = Input.GetAxis("Vertical");             GetAxis with smoothdamp
        Dright = Input.GetAxis("Horizontal");
        */
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0)  - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        
        // player can't input
        if(!inputEnable)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);


        //Vector2 tmp = new Vector2(Dup,Dright);
        //tmp.Normalize();
        //Dmag = tmp.magnitude > 0.01f ? tmp.magnitude : 0;
        //Debug.Log(tmp.magnitude);
        
        /**
         * square vec to circle vec
         */
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float tempDright = tempDAxis.x;
        float tempDup = tempDAxis.y;

        Dmag = Mathf.Sqrt(tempDright*tempDright + tempDup*tempDup);
        Vector3 movRight = Vector3.ProjectOnPlane(cameraHandle.transform.right, Vector3.up);
        Vector3 movForward = Vector3.ProjectOnPlane(cameraHandle.transform.forward, Vector3.up);
        Dvec = tempDright * movRight + tempDup * movForward;
        //Debug.Log("Dup: " + Dup);

        run = Input.GetKey(keyA);

        isJump = Input.GetKeyDown(keyB);

        isAttack = Input.GetKeyDown(keyC);
        

        /**
         * mouse control view
         */
        viewDRight = Input.GetAxisRaw("Mouse X");
        viewDUp = Input.GetAxisRaw("Mouse Y");

        /**
         * for GetKeyDown
         */
        //if (tempJump != lastJump && tempJump)
        //{
        //    isJump = true;
        //    Debug.Log("Jump Tigger!!!!!");
        //}
        //else
        //{
        //    isJump = false;
        //}
        //lastJump = tempJump;

    }

    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

    //    return output;
    //}
}
