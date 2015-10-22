using UnityEngine;
using System.Collections;

public class AninTestControl : MonoBehaviour
{
    Animator animtr;
    Rigidbody rgdBdy;
    bool OnAir;

    Renderer rndr;

    // Use this for initialization
    void Start()
    {
        animtr = GetComponentInChildren<Animator>();
        rgdBdy = GetComponent<Rigidbody>();
        OnAir = false;

        rndr = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //animtr.SetInteger("YVelocity", Mathf.RoundToInt(rgdBdy.velocity.y));

        if (Input.GetKey(KeyCode.A))
        {
            animtr.SetBool("Moving", true);
            Debug.Log("Moving");
        }
        else
        {
            animtr.SetBool("Moving", false);
        }
         
        if (Input.GetKeyDown(KeyCode.S))
        {
            animtr.SetTrigger("LightAttack");
            Debug.Log("LightAttack");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            animtr.SetTrigger("HeavyAttack");
            Debug.Log("HeavyAttack");
        }

        if (Input.GetKey(KeyCode.F))
        {
            animtr.SetBool("isDashing", true);
            Debug.Log("isDashing");
        }
        else
        {
            animtr.SetBool("isDashing", false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            animtr.SetTrigger("Jump");
            OnAir = !OnAir;
            animtr.SetBool("OnAir", OnAir);
            Debug.Log("Jump");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //rndr.material.mainTexture = guitarTexture;
        }
    }
}
