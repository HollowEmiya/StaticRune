using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Material material;
    public Renderer objRenderer;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        material = objRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
