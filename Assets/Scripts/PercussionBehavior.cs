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

    private float h = 0.0f;
    private float v = 0.0f;

    private bool isMoving;
    private int frameCount;

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

        //if (Input.GetButtonDown("B"))
        //{
        //    isMoving = true;
        //}

        //if (Input.GetButtonUp("B"))
        //{
        //    isMoving = false;
        //}

        //if (GetDPadRightButtonDown())
        //{
        //    h = 1.0f;
        //    v = 0.0f;
        //}

        //if (GetDPadUpButtonDown())
        //{
        //    h = 0.0f;
        //    v = 1.0f;
        //}


        if (Input.GetButtonDown("B") || GetDPadRightButtonDown())
        {
            //rbody.AddForceAtPosition(Vector3.up * 50.0f, transform.position - Vector3.up + Vector3.right);
            rbody.AddForce(Vector3.right * 100, ForceMode.Force);
            //  rbody.AddRelativeTorque(transform.right * 100, ForceMode.Force);
            //isMoving = true;
        }

        if (Input.GetButtonDown("Y") || GetDPadUpButtonDown())
        {
            //rbody.AddForceAtPosition(Vector3.up * 50.0f, transform.position - Vector3.up + Vector3.right);
            rbody.AddRelativeForce(transform.up * 100, ForceMode.Force);
            // rbody.AddRelativeTorque(-transform.up, ForceMode.Force);
            //isMoving = true;
        }

        //if (GetDPadUpButton())
        //{
        //    Debug.Log("UP");
        // }

        //if (isMoving)
        //{

        //    transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        //}
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
