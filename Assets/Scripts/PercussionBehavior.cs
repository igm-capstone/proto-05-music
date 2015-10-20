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
    }

    void FixedUpdate()
    {
        


    }
}
