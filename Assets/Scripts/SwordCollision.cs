using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [Header("Settings")]
    public float attackForce = 15;
    public LayerMask collLayerMask;
    public Vector3 hitPoint;
    public Animator anim;

    [Space(5)]
    [Header("Particle")]
    public GameObject hitParticle;
    public GameObject stasisHitParticle;


    private BoxCollider boxCol;
    public GameObject debugBall;
    private GameObject debugBallPtr;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider>();
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        boxCol.enabled = anim.GetBool("collider");
    }

    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        StasisObject1Mat rig;
        if (Physics.Raycast(transform.position + (transform.forward * 1.0f), -transform.forward, out hit, 8, collLayerMask))
        {
            //if(debugBallPtr != null)
            //{
            //    Destroy();
            //}debugBallPtr
            // print(hit.point);
            //debugBallPtr = Instantiate(debugBall,hit.point,Quaternion.identity);
            rig = other.gameObject.GetComponent<StasisObject1Mat>();
            if(rig != null)
            {
                
                if (rig.beStasised)
                {
                    //print(forceDir);
                    rig.AddToForce(hit.point,attackForce);
                    Instantiate(stasisHitParticle,hit.point,Quaternion.identity);
                }
                else
                {
                    rig.rb.AddForce(rig.computeDir(hit.point)*attackForce);
                    Instantiate(hitParticle,hit.point,Quaternion.identity);
                }
            }
        }        
        //print(LayerMask.LayerToName( other.gameObject.layer));
    }
}
