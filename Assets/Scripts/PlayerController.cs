using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData parametersPlayer;

    private IPlayerInput input;
    private IMovable move;
    private IJumper jump;
    private IAnimator anim;

    private Rigidbody2D rb;
    private Animator animator;
    private bool enable = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        input = new PlayerInput();
        move = new PlayerMove(rb, parametersPlayer, transform);
        jump = new PlayerJump(rb, parametersPlayer, transform);
        anim = new PlayerAnimator(animator);
    }
    private void Update()
    {
        if (enable) return;
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            animator.SetTrigger("Idle");
        } 
            
        input.Update();

        bool isWall = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(input.MoveInput), parametersPlayer.wallCheckDistance, parametersPlayer.wallLayer);
        anim.MoveAnimations(input.RunPressed, isWall ? 0 : input.MoveInput);
    }

    private void FixedUpdate()
    {
        if (enable) return;
        move.Move(input.MoveInput, input.RunPressed);
    }
    public void Enable(bool disable) // немного напутала true = выкл, false вкл
    {
        anim.MoveAnimations(false, 0);
        rb.velocity = Vector3.zero;
        enable = disable;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 wallCheckDir = transform.right * parametersPlayer.wallCheckDistance;
        Gizmos.DrawLine(transform.position, transform.position + wallCheckDir);
        Gizmos.DrawLine(transform.position, transform.position - wallCheckDir);
    }
}
