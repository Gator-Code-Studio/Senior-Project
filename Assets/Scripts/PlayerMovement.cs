using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    private float horizontal;
    [SerializeField] float speed = 8f;
    [SerializeField] float jumpingPower = 16f;
    private bool isFacingRight = true;

    // Jump Timing
    [SerializeField] float coyoteTime = .2f;
    private float coyoteTimeCounter;
    private bool doubleJump;
    private float jumpBufferTime = .2f;
    private float jumpBufferCounter;

    // Wall
    private bool isWallSliding;
    [SerializeField] float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] float wallJumpingTime = .2f;
    private float wallJumpingCounter;
    [SerializeField] float wallJumpingDuraction = .4f;
    [SerializeField] Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Dash
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = .2f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] private float diagonalVerticalScale = .6f;
    [SerializeField] private float verticalDashScale = 0.8f;
    [SerializeField] private float diagonalDashScale = 0.9f;

    private bool wasGrounded;

    // Refs
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Animator animator;

    // Animator state
    private bool isRunning;
    private bool isJumping;
    private bool isJumpingUp;
    private bool isJumpingDown;
    private bool movingKeyPressed;

    // Update
    void Update()
    {
        bool grounded = IsGrounded();

        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
            if (!wasGrounded)
            {
                doubleJump = false; // reset only when you just landed
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (isDashing)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        horizontal = Input.GetAxis("Horizontal");

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            doubleJump = true;
            jumpBufferCounter = 0f;
        }

        if (jumpBufferCounter > 0f && !IsGrounded() && doubleJump && !isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            doubleJump = false;
            jumpBufferCounter = 0f;
        }

        wasGrounded = grounded;

        if (Input.GetButtonUp("Jump")) jumpBufferCounter = 0f;

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * .5f);
            coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }

        movingKeyPressed =
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        isRunning = movingKeyPressed && IsGrounded() && !isDashing && !isWallSliding;
        isJumping = !IsGrounded() && rb.linearVelocity.y > 0f || isWallJumping;

        if (isWallSliding)
        {
            isJumpingUp = false;
            isJumpingDown = false;
        }
        else
        {
            isJumpingUp = !IsGrounded() && rb.linearVelocity.y > 0f;
            isJumpingDown = !IsGrounded() && rb.linearVelocity.y < 0f;
        }

        isJumpingUp = !IsGrounded() && rb.linearVelocity.y > 0f;
        isJumpingDown = !IsGrounded() && rb.linearVelocity.y < 0f;

        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isJumpingUp", isJumpingUp);
            animator.SetBool("isJumpingDown", isJumpingDown);
            animator.SetBool("isDashing", isDashing);
            animator.SetBool("isWallSliding", isWallSliding);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (movingKeyPressed && !isWallSliding)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsWalled()
    {
        Console.Write("IsWalled");
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue)
            );

            if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(
                wallJumpingDirection * wallJumpingPower.x,
                wallJumpingPower.y
            );
            wallJumpingCounter = 0f;
            doubleJump = true;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuraction);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);
        if (dir.sqrMagnitude > 0f)
        {
            dir = dir.normalized;
        }
        else
        {
            dir = new Vector2(transform.localScale.x, 0f);
        }

        bool hasX = Mathf.Abs(x) > 0f;
        bool hasY = Mathf.Abs(y) > 0f;

        if (hasX && hasY)
        {
            dir.y *= diagonalVerticalScale;
            dir = dir.normalized;
        }

        if (IsGrounded() && dir.y < 0f)
        {
            dir.y = 0f;
        }

        float power = dashingPower;
        if (!hasX && hasY)
        {
            power *= verticalDashScale;
        }
        else if (hasX && hasY)
        {
            power *= diagonalDashScale;
        }

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = dir * power;
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
