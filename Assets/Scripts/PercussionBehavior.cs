using UnityEngine;
using System.Collections;



public class PercussionBehavior : MonoBehaviour
{
    private const float DPAD_UP     = +1.0f;
    private const float DPAD_DOWN   = -1.0f;
    private const float DPAD_RIGHT  = +1.0f;
    private const float DPAD_LEFT   = -1.0f;

    private float prevDPadX;
    private float prevDPadY;
    private float dPadX;
    private float dPadY;

    public float JumpSpeed      = 100.0f;
    public float MovementSpeed  = 20.0f;

    public Rigidbody rbody;
	// Use this for initialization
	void Start ()
    {
        rbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        prevDPadX = dPadX;
        prevDPadY = dPadY;

        dPadX = Input.GetAxis("DPadX");
        dPadY = Input.GetAxis("DPadY");

        Vector3 velocity = Vector3.zero;
        if (GetDPadRightButton())
        {
            velocity = Vector3.right;
        }

        if (GetDPadLeftButton())
        {
            velocity = -Vector3.right;
        }


        rbody.velocity = velocity * MovementSpeed;

        if (Input.GetButtonDown("A"))
        {
            rbody.AddForce(Vector3.up * JumpSpeed, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        


    }

    #region DPad Controls
    bool GetDPadUpButton()
    {
        return (dPadY == DPAD_UP);
    }

    bool GetDPadUpButtonDown()
    {
        return (prevDPadY != DPAD_UP && dPadY == DPAD_UP);
    }

    bool GetDPadUpButtonUp()
    {
        return (prevDPadY == DPAD_UP && dPadY != DPAD_UP);
    }

    bool GetDPadDownButton()
    {
        return (dPadY == DPAD_DOWN);
    }

    bool GetDPadDownButtonDown()
    {
        return (prevDPadY != DPAD_DOWN && dPadY == DPAD_DOWN);
    }

    bool GetDPadDownButtonUp()
    {
        return (prevDPadY == DPAD_DOWN && dPadY != DPAD_DOWN);
    }

    bool GetDPadRightButton()
    {
        return (dPadX == DPAD_RIGHT);
    }

    bool GetDPadRightButtonDown()
    {
        return (prevDPadX != DPAD_RIGHT && dPadX == DPAD_RIGHT);
    }

    bool GetDPadRightButtonUp()
    {
        return (prevDPadX == DPAD_RIGHT && dPadX != DPAD_RIGHT);
    }

    bool GetDPadLeftButton()
    {
        return (dPadX == DPAD_LEFT);
    }

    bool GetDPadLeftButtonDown()
    {
        return (prevDPadX != DPAD_LEFT && dPadX == DPAD_LEFT);
    }

    bool GetDPadLeftButtonUp()
    {
        return (prevDPadX == DPAD_LEFT && dPadX != DPAD_LEFT);
    }
    #endregion
}
