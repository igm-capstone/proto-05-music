using UnityEngine;
using System.Collections;

public class AninTestControl : MonoBehaviour
{
    Animator animtr;
    Rigidbody rgdBdy;
    bool OnAir;

    // Use this for initialization
    void Start()
    {
        animtr = GetComponentInChildren<Animator>();
        rgdBdy = GetComponent<Rigidbody>();
        OnAir = false;
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            animtr.SetTrigger("Dash");
            Debug.Log("Dash");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            animtr.SetTrigger("Jump");
            OnAir = !OnAir;
            animtr.SetBool("OnAir", OnAir);
            Debug.Log("Jump");
        }
    }
}
