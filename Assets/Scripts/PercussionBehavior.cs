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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        prevDPadX = dPadX;
        prevDPadY = dPadY;

        dPadX = Input.GetAxis("DPadX");
        dPadY = Input.GetAxis("DPadY");
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
