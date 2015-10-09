using UnityEngine;
using System.Collections;

public class AninTestControl : MonoBehaviour
{
    Animator animtr;
    bool isFacingRight;

    // Use this for initialization
    void Start()
    {
        animtr = GetComponentInChildren<Animator>();

        if (transform.localScale.y > 0) isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.D))
        {
            animtr.SetBool("Moving", true);
            transform.position += new Vector3(0.1f, 0);
            if (!isFacingRight) FlipModel();
        }

        else if (Input.GetKey(KeyCode.A))
        {
            animtr.SetBool("Moving", true);
            transform.position -= new Vector3(0.1f, 0);
            if (isFacingRight) FlipModel();
        }

        else
        {
            animtr.SetBool("Moving", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animtr.SetBool("Attack1Trigger", true);
        }
        else
        {
            animtr.SetBool("Attack1Trigger", false);
        }
    }

    void FlipModel()
    {
        transform.localScale = new Vector3(-1.0f * transform.localScale.x, 
                                           transform.localScale.y,
                                           transform.localScale.z);
        isFacingRight = !isFacingRight;
    }

}
