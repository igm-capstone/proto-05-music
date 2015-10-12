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
    bool hasWallJump;
    float jumpTimeout;

    // grip parameters
    public AnimationCurve gripCurve;
    public float gripForce = -25f;
    public float gripDuration = 1f;
    public bool canWallJump = true;
    float remainingGripDuration;

    private Transform model;
    private Animator animator;
    private CharacterController2D controller;

    void Awake()
    {
        model = transform.Find("Model");
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    void Start()
    {
        controller.onTriggerEnterEvent += TriggerEnterEvent;
    }

    void TriggerEnterEvent(Collider2D other)
    {
        if (other.tag == "Audio")
        {
            if (other.GetComponent<CollectibleBehavior>() != null)
                other.GetComponent<CollectibleBehavior>().PlayAudio(gameObject.GetComponent<Collider2D>());
        }

        if (other.tag == "Enemy")
        {
            if (gameObject.GetComponent<AttackEvent>() != null)
                gameObject.GetComponent<AttackEvent>().TriggerEvent(other.GetComponent<Collider2D>());
        }
    }

    void Update()
    {
        var isMovingLeft = Input.GetKey(KeyCode.LeftArrow);
        var isMovingRight = Input.GetKey(KeyCode.RightArrow);
        var isMoving = isMovingLeft || isMovingRight;

        var acceleration = airAcceleration;
        var velocity = controller.velocity;
        if (controller.isGrounded)
        {
            velocity.y = 0;
            acceleration = groundAcceleration;
            remainingGripDuration = gripDuration;

            if (canDoubleJump)
            {
                hasDoubleJump = true;
            }
        }


        // Inform animator if character is airborne;
        animator.SetBool("OnAir", !controller.isGrounded);

        if (isMovingRight)
        {
            // TODO: next time, try using SmoothDump
            velocity.x = Mathf.Lerp(velocity.x, RunSpeed, acceleration * Time.deltaTime);
            
            if (controller.isGrounded)
            {
                animator.SetBool("Moving", true);
                //animator.Play(run);
            }

            var euler = model.eulerAngles;
            euler.y = 90;
            model.eulerAngles = euler;
        }
        else if (isMovingLeft)
        {
            velocity.x = Mathf.Lerp(velocity.x, -RunSpeed, acceleration * Time.deltaTime);

            if (controller.isGrounded)
            {
                animator.SetBool("Moving", true);
                //animator.Play(run);
            }

            var euler = model.eulerAngles;
            euler.y = -90;
            model.eulerAngles = euler;
        }
        else
        {
            animator.SetBool("Moving", false);
            velocity.x = Mathf.Lerp(velocity.x, 0, acceleration * Time.deltaTime);
        }

        if (!isAboutToJump && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (controller.isGrounded)
            {
                jumpTimeout = jumpPrepTime;
                animator.SetTrigger("Jump");
                isAboutToJump = true;
                remainingGripDuration = gripDuration;
            }
            else if (hasWallJump)
            {
                jumpTimeout = 0;
                animator.SetTrigger("Jump");
                isAboutToJump = true;
                hasWallJump = false;
                remainingGripDuration = gripDuration;
                velocity.x = RunSpeed * (controller.collisionState.lastGripLeft ? 1 : -1);

                Debug.Log("Wall");
            }
            else if (hasDoubleJump)
            {
                jumpTimeout = 0;
                animator.SetTrigger("Jump");
                isAboutToJump = true;
                hasDoubleJump = false;
                remainingGripDuration = gripDuration;

                Debug.Log("Double");
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

        if (isMoving && controller.isGripped && velocity.y < 0)
        {
            var grip = gripCurve.Evaluate(1 - remainingGripDuration / gripDuration) * gripForce;
            velocity.y = Mathf.Lerp(velocity.y, 0, grip * Time.deltaTime);
            
            remainingGripDuration = Mathf.Clamp(remainingGripDuration - Time.deltaTime, 0, gripDuration);

            if (canWallJump)
            {
                hasWallJump = true;
                hasDoubleJump = false;
            }
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
