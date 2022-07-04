using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyControl : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        // BETOP is axis4 and axis5, aixs4 is horizontal, aixs5 is vertical 
        if(Input.GetAxisRaw("JoyAxis3")!=0)
        {
            print("JoyAxis3" + Input.GetAxisRaw("JoyAxis3"));
        }
        if (Input.GetAxisRaw("JoyAxis7") != 0)
        {
            print("JoyAxis7" + Input.GetAxisRaw("JoyAxis7"));
        }
        for (int i = 0; i < 8; i++)
        {
            string joyBut = "JoyBut" + i.ToString();
            if(Input.GetButtonDown(joyBut))
            {
                print(joyBut + "!!!!");
            }
        }
    }
}
