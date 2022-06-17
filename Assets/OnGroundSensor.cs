using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;
    public float capcolOffset = 0.1f;

    private Vector3 point1;
    private Vector3 point2;
    private float radius;

    private void Awake()
    {
        radius = capcol.radius - 0.01f;
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
        point1 = transform.position + transform.up * (radius - capcolOffset);
        point2 = transform.position + transform.up * (capcol.height - radius - capcolOffset);

        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if (outputCols.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
