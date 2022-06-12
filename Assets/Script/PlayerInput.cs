using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
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

    [Header("===== Output Siganl =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;

    // 1.pressing signal
    public bool run;
    // 2. trigger once type siganl
    // 3. double trigger

    [Header("===== Others =====")]
    public bool inputEnable = true;

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Dmag = Mathf.Sqrt(Dup * Dup + Dright * Dright);
        Dvec = Dright * transform.right + Dup * transform.forward;
        //Debug.Log("Dup: " + Dup);

        run = Input.GetKey(keyA);
    }
}
