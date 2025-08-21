using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformControllerAdv : MonoBehaviour
{
    PlayerInput playerInput;

    private Rigidbody2D rb;
    public float speed = 3f;
    private float inputX;
    bool facingRight = true;

    [InspectorLabel("Ground detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer = (1 << 6);
    [SerializeField] private bool isGrounded;

    [InspectorLabel("Extended Jumping")]
    [SerializeField] float jumpForce = 5f;
    bool isJumping;
    [SerializeField] float jumpTime = 0.5f;
    float jumpTimeLeft;

    [InspectorLabel("Coyote time and jumpbuffer")]
    [SerializeField] private float coyoteTime = 0.2f;
    float coyoteTimeLeft;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float jumpBufferCounter;
    [SerializeField] ParticleSystem jumpEffect;
    private Animator anim;
    private bool useAnim = true;
    bool jump = false;

    [InspectorLabel("Wall Sliding")]
    [SerializeField] bool isWallSliding;
    [SerializeField] float wallSlidingSpeed = 2f;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer = (1 << 7);
    [SerializeField] private Vector2 wallCheckRadius = new Vector2(0.2f, 1.6f);

    [InspectorLabel("Wall Jumping")]
    bool isWallJumping;
    float wallJumpDir;
    float wallJumpingTime = 0.2f;
    float wallJumpingCounter;
    float wallJumpingDuration = 0.2f;
    [SerializeField] Vector2 wallJumpingPower = new Vector2(4f, 8f);


    [InspectorLabel("Dashing")]
    bool canDash = true;
    [SerializeField] bool isDashing = false;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] TrailRenderer trail;

    private void Awake()
    {
        playerInput = new PlayerInput();
        
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (TryGetComponent<Animator>(out Animator a))
        {
            anim = a;
        }
        else
        {
            useAnim = false;
        }

    }
    //----- Input methods -----\\
    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
    }
    public void Jump (InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            jump = true;
            //jumpBufferCounter = jumpBufferTime;
        }
        if (context.canceled)
        {
            jump = false;
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    //----- Physics update -----\\
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (!isWallJumping)
        {
            MovingPlayer();
        }
        WallSlide();
        WallJump();
        PlayerJump();
    }

    //----- Movement -----\\
    private void MovingPlayer()
    {
        //Use animations
        if (useAnim)
        {
            anim.SetFloat("Speed", Mathf.Abs(inputX));
        }

        CheckDirection();

        rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);

        Collider2D[] physicsHit = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);
        if (physicsHit.Length > 0)
        {
            foreach (Collider2D col in physicsHit)
            {
                if (col.gameObject != this.gameObject)
                {
                    isGrounded = true;
                    break;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * 1.5f * Time.fixedDeltaTime);
        }
    }
    //----- Jumping methods -----\\
    private void PlayerJump()
    {
        //Coyote time 
        if (isGrounded)
        {
            coyoteTimeLeft = coyoteTime;
        }
        else
        {
            coyoteTimeLeft -= Time.deltaTime;
        }

        //Jump buffer (for missclicked jumps)
        if (jump)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        //Jump mechanics
        if (jumpBufferCounter > 0f && coyoteTimeLeft > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpEffect.Play();
            isJumping = true;
            jumpTimeLeft = jumpTime;
            coyoteTimeLeft = 0;
            jumpBufferCounter = 0;

        }
        //Holding jump (longer jumps)
        if (jump && isJumping)
        {
            if (jumpTimeLeft > 0)
            {
                jumpTimeLeft -= Time.deltaTime;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
        //stop jumping
        if (!jump)
        {
            isJumping = false;
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDir = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (jump && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if(transform.localScale.x != wallJumpDir)
            {
                Flip();
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private bool isWalled()
    {
        Collider2D[] overlapHit = Physics2D.OverlapCircleAll(wallCheck.position, groundCheckRadius, wallLayer);
        if (overlapHit.Length > 0)
        {
            foreach (Collider2D col in overlapHit)
            {
                if (col.gameObject != this.gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void WallSlide()
    {
        if (isWalled() && !isGrounded && inputX != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocityY, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    //----- Dash -----\\
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float orgGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        rb.gravityScale = orgGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    //----- Character direction -----\\
    void CheckDirection()
    {
        if (inputX < 0 && facingRight)
        {
            Flip();
        }
        if (inputX > 0 && !facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;
    }

}