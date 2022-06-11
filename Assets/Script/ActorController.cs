using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;

    [SerializeField]            // 让变量可见，该变量必须被编译器支持
    private Animator anim;

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();           // get by itself
        anim = model.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("forward", pi.Dmag );
        if(pi.Dvec.magnitude > 0.01f )
            anim.transform.forward = pi.Dvec;
    }
}
