using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{
    [Header("Joy stick setting")]
    public string axisX = "AxisX";
    public string axisY = "AxisY";
    public string axisViewRight = "JoyAxis4";  // view move horizontal
    public string axisViewUp = "JoyAxis5";     // view move vertical
    public string btn0 = "JoyBtn0";
    public string btn1 = "JoyBtn1";
    public string btn3 = "JoyBtn3";

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
    //Start is called before the first frame update
    void Start()
    {
        cameraHandle = GetComponentInChildren<CameraController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        viewDUp = -1 * Input.GetAxis(axisViewUp);
        viewDRight = Input.GetAxis(axisViewRight);

        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);

        // player can't input
        if (!inputEnable)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        /**
         * square vec to circle vec
         */
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float tempDright = tempDAxis.x;
        float tempDup = tempDAxis.y;

        Dmag = Mathf.Sqrt(tempDright * tempDright + tempDup * tempDup);
        Vector3 movRight = Vector3.ProjectOnPlane(cameraHandle.transform.right, Vector3.up);
        Vector3 movForward = Vector3.ProjectOnPlane(cameraHandle.transform.forward, Vector3.up);
        Dvec = tempDright * movRight + tempDup * movForward;
        
        run = Input.GetButton(btn0);
        isJump = Input.GetButtonDown(btn3);
        isAttack = Input.GetButtonDown(btn1);
    }

    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

    //    return output;
    //}
}
