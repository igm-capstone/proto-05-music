using UnityEngine;
using System.Collections;
using Prime31;
using UnityStandardAssets.ImageEffects;

public class PlayerController : MonoBehaviour
{
    private const float DPAD_UP = +1.0f;
    private const float DPAD_DOWN = -1.0f;
    private const float DPAD_RIGHT = +1.0f;
    private const float DPAD_LEFT = -1.0f;

    private float prevDPadX;
    private float prevDPadY;
    private float dPadX;
    private float dPadY;

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

    // dash parameters
    private AnimationCurve dashCurve;
    private Vector2 dashFrom;
    private Vector2 dashTo;
    private float dashDuration;
    private float elapsedDashDuration;
    private bool isDashing;

    private Transform model;
    private Animator animator;
    private CharacterController2D controller;
    public bool eventCheck = false;
    private AudioSource collectible;

    private float timeSinceLastCollectible = 0.0f;
    private GameObject camera;
    private float offset;
    private bool collected = false;

    void Awake()
    {
        model = transform.Find("Model");
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    void Start()
    {
        controller.onTriggerEnterEvent += TriggerEnterEvent;
        camera = GameObject.Find("Main Camera");
    }

    void TriggerEnterEvent(Collider2D other)
    {
        var audio = other.GetComponent<CollectibleBehavior>();
        if (audio != null)
        {
            collectible = other.gameObject.GetComponent<AudioSource>();
            audio.PlayAudio(gameObject.GetComponent<Collider2D>());
            timeSinceLastCollectible = 0.0f;
            offset = Time.time;
            collected = true;
        }

        if (other.tag == "Enemy") { 
            var attack = gameObject.GetComponent<AttackEvent>();
            if (attack != null)
                attack.TriggerEvent(other.GetComponent<Collider2D>());
        }

        var dash = other.GetComponent<DashTrigger>();
        if (dash != null)
            dash.StartDash(gameObject.GetComponent<Collider2D>());

        if (other.tag == "Goal")
        {
            GameObject.FindObjectOfType<ComposerBehaviour>().PlayComposition();
            GameObject.FindObjectOfType<GoalBehavior>().GoToNextLevel(this.gameObject);
        }
    }

    public void DashTo(Vector2 target, float duration, AnimationCurve curve = null)
    {
        dashCurve = curve ?? AnimationCurve.Linear(0, 0, 1, 1);
        isDashing = true;
        dashDuration = duration;
        elapsedDashDuration = 0;
        dashTo = target;
        dashFrom = transform.position;

        animator.SetBool("isDashing", true);
    }

    void Update()
    {
        timeSinceLastCollectible += Time.deltaTime;
        if (collectible != null)
        {           
            if (timeSinceLastCollectible  >= 0)
            {
                float value = Mathf.Lerp(-1f, 1, (Time.time - offset - (collectible.clip.length + 2)) * 0.25f);
                camera.GetComponent<Grayscale>().changeEffectAmount(value);
                collected = false;
               // Debug.Log(value);
            }
        }
        if (collected)
        {
            //if the color is bw and the player collects a new collectible.
            if (camera.GetComponent<Grayscale>().effectAmount >= -1f)
            {
                float temp = camera.GetComponent<Grayscale>().effectAmount;
                float value = Mathf.Lerp(temp, -1, (Time.time - offset) * 0.5f);
                camera.GetComponent<Grayscale>().changeEffectAmount(value);
                Debug.Log(value);
            }
        }
        //var horizontal = Input.GetAxis("Horizontal");
        //var isMovingLeft = horizontal < 0f;
        //var isMovingRight = horizontal > 0f;
        //var isMoving = isMovingLeft || isMovingRight;
        prevDPadX = dPadX;
        prevDPadY = dPadY;

        dPadX = Input.GetAxis("DPadX");
        dPadY = Input.GetAxis("DPadY");
        Debug.Log("DPad X: " + dPadX);
        Debug.Log("DPad Y: " + dPadY);


        var isMovingLeft = Input.GetKey(KeyCode.LeftArrow) || (GetDPadLeftButton());
        var isMovingRight = Input.GetKey(KeyCode.RightArrow) || (GetDPadRightButton());
        var isMoving = isMovingLeft || isMovingRight;

        if (isDashing)
        {

            float t = elapsedDashDuration / dashDuration;

            if (t >= 1)
            {
                t = 1;
                isDashing = false;
                animator.SetBool("isDashing", false);
            }


            var newPosition = Vector3.Lerp(dashFrom, dashTo, dashCurve.Evaluate(t));
            var deltaMovement = newPosition - transform.position;

            controller.Move(deltaMovement);
            elapsedDashDuration += Time.deltaTime;

            return;
        }


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

        if (!isAboutToJump && (Input.GetButtonDown("Jump") || Input.GetButtonDown("A")))
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
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("A"))
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

        //animator.SetFloat("FallSpeed", velocity.y);
        //animator.SetBool("IsGrounded", controller.isGrounded);
        
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

        if(!eventCheck)
            controller.Move(velocity * Time.deltaTime);
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
