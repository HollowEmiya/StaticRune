using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("===== Output Siganl =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    /**
     * <summary> move direction </summary>
     */
    public Vector3 Dvec;
    public float viewDUp;
    public float viewDRight;

    // 1.pressing signal
    public bool run;
    public bool staticRune;
    // 2. trigger once type siganl
    public bool isJump;
    public bool isAttack;
    // private bool lastJump;           // for simulate Input.GetKeyDown()
    // 3. double trigger

    [Header("===== Others =====")]
    public bool inputEnable = true;
    [SerializeField]
    protected GameObject cameraHandle;
    public bool mouseIsVisible = false;

    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }
}
