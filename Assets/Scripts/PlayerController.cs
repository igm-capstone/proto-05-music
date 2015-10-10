using UnityEngine;
using System.Collections;
using Prime31;

public class PlayerController : MonoBehaviour
{

    public float Gravity = -15f;
    public float RunSpeed = 8f;

    // Drag
    public float airAcceleration = 10f;
    public float groundAcceleration = 50f;

    // jump parameters
    public float shortJumpHeight = 3f;
    public float longJumpHeight = 5f;
    public float jumpPrepTime = 0.15f;
    public bool canDoubleJump = true;
    bool isAboutToJump;
    bool isShortJump;
    bool hasDoubleJump;
    float jumpTimeout;
    public GameObject Model;

    private Animator animator;
    private CharacterController2D controller;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        var acceleration = airAcceleration;
        var velocity = controller.velocity;
        if (controller.isGrounded)
        {
            velocity.y = 0;
            acceleration = groundAcceleration;

            if (canDoubleJump)
            {
                hasDoubleJump = true;
            }

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x = Mathf.Lerp(velocity.x, RunSpeed, acceleration * Time.deltaTime);
            
            if (controller.isGrounded)
            {
                Model.GetComponent<Animator>().SetBool("Moving", true);
                //animator.Play(run);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.x = Mathf.Lerp(velocity.x, -RunSpeed, acceleration * Time.deltaTime);

            if (controller.isGrounded)
            {
                Model.GetComponent<Animator>().SetBool("Moving", true);
                //animator.Play(run);
            }
        }
        else
        {
            Model.GetComponent<Animator>().SetBool("Moving", false);
            velocity.x = Mathf.Lerp(velocity.x, 0, acceleration * Time.deltaTime);
        }

        if (!isAboutToJump && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (controller.isGrounded)
            {
                jumpTimeout = jumpPrepTime;
                animator.SetTrigger("Jump");
                isAboutToJump = true;
            }
            else if (hasDoubleJump)
            {
                jumpTimeout = 0;
                animator.SetTrigger("Jump");
                isAboutToJump = true;
                hasDoubleJump = false;
            }
        }
        
        if (isAboutToJump)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                // short jump!
                isShortJump = true;
                animator.SetBool("ShortJump", true);
            }
            else if (jumpTimeout <= 0)
            {
                var jumpHeight = isShortJump ? shortJumpHeight : longJumpHeight;
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -Gravity);
                isAboutToJump = false;
                isShortJump = false;
                animator.SetBool("ShortJump", false);
            }

            jumpTimeout -= Time.deltaTime;
        }

        animator.SetFloat("FallSpeed", velocity.y);
        animator.SetBool("IsGrounded", controller.isGrounded);

        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
