using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : IAnimator
{
    private Animator animator;
    public PlayerAnimator(Animator _animator)
    {
        animator = _animator;
    }
    public void MoveAnimations(bool run, float speed)
    {
        animator.SetFloat("XSpeed", Mathf.Abs(speed));
    }
    public void JumpAnimations(bool ground, float speed)
    {
        animator.SetBool("Land", ground);
        animator.SetFloat("YSpeed", speed);
    }
}
public class PlayerInput : IPlayerInput
{
    public float MoveInput { get; private set; }
    public bool RunPressed { get; private set; }
    public bool JumpPressed { get; private set; }
    public void Update()
    {
        MoveInput = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetButtonDown("Jump");
        RunPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
}
public class PlayerMove : IMovable
{
    private readonly Rigidbody2D rb;
    private readonly PlayerData parameters;
    private readonly Transform playerTransform;

    public PlayerMove(Rigidbody2D _rb, PlayerData _parameters, Transform _playerTransform)
    {
        rb = _rb;
        parameters = _parameters;
        playerTransform = _playerTransform;
    }

    public void Move(float direction, bool isRunning)
    {
        // Проверка на стену
        bool isWall = CheckWall(direction);
        if (isWall)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        if (direction != 0)
        {
            float scaleX = Mathf.Sign(direction) * Mathf.Abs(playerTransform.localScale.x);
            playerTransform.localScale = new Vector3(scaleX, playerTransform.localScale.y, playerTransform.localScale.z);
        }

        float currentSpeed = isRunning ? parameters.runSpeed : parameters.walkSpeed;
        float targetSpeed = direction * currentSpeed;

        float smoothedSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, parameters.moveAcceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(smoothedSpeed, rb.velocity.y);
    }
    private bool CheckWall(float direction)
    {
        if (direction == 0) return false;

        Vector2 rayOrigin = playerTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * Mathf.Sign(direction), parameters.wallCheckDistance, parameters.wallLayer);

        Debug.DrawRay(rayOrigin, Vector2.right * Mathf.Sign(direction) * parameters.wallCheckDistance, Color.red);

        return hit.collider != null;
    }
}
public class PlayerJump : IJumper
{
    private readonly Rigidbody2D rb;
    private readonly PlayerData parameters;
    private readonly Transform pos;

    private float lastGroundedTime;
    private float lastJumpInputTime;
    private bool isJumping;
    private bool isFalling;

    public PlayerJump(Rigidbody2D _rb, PlayerData _parameters, Transform _pos)
    {
        rb = _rb;
        pos = _pos;
        parameters = _parameters;
    }

    public void SetJumpInput(bool jumpPressed)
    {
        if (jumpPressed)
        {
            lastJumpInputTime = Time.time;
        }
    }

    public void TryJump(bool move)
    {
        bool isGrounded = IsGrounded();
        if (isGrounded) lastGroundedTime = Time.time;

        bool canJump = (Time.time - lastGroundedTime <= parameters.jumpCoyoteTime) && (Time.time - lastJumpInputTime <= parameters.jumpBufferTime) && !isJumping;

        if (canJump)
        {
            if (move) rb.velocity = new Vector2(rb.velocity.x, parameters.jumpSpeed);
            else rb.velocity = new Vector2(0, parameters.jumpSpeed);
            isJumping = true;
            lastJumpInputTime = 0;
        }
        else isJumping = false;

        if (isGrounded && rb.velocity.y <= 0) isFalling = false;
    }
    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(pos.position, parameters.groundCheckRadius, parameters.groundLayer))
            return true;

        float edgeOffset = parameters.groundCheckRadius * 0.9f;
        Vector2 leftCheck = pos.position - new Vector3(edgeOffset, 0);
        Vector2 rightCheck = pos.position + new Vector3(edgeOffset, 0);

        return Physics2D.OverlapCircle(leftCheck, parameters.groundCheckRadius / 2, parameters.groundLayer) ||
               Physics2D.OverlapCircle(rightCheck, parameters.groundCheckRadius / 2, parameters.groundLayer);
    }
    public bool IsFall()
    {
        return isFalling;
    }
}
