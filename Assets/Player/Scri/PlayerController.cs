using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 20f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // Movement variables
    private float horizontalInput;
    private bool isGrounded;
    private bool facingRight = true;
    
    // Animation parameter names
    private const string ANIM_IDLE = "Idle";
    private const string ANIM_RUN = "Run";
    private const string ANIM_JUMP = "Jump";
    private const string ANIM_BLINK = "Blink";
    
    // Blink variables
    [Header("Blink Settings")]
    [SerializeField] private float minBlinkInterval = 2f;
    [SerializeField] private float maxBlinkInterval = 5f;
    private float nextBlinkTime;
    private bool isBlinking = false;
    private float blinkDuration = 0.3f; // Blink 애니메이션 길이
    
    void Start()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Create ground check if not assigned
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.parent = transform;
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObj.transform;
        }
        
        // Initialize blink timer
        SetNextBlinkTime();
    }
    
    void Update()
    {
        // 1. 입력 감지 코드를 삭제했습니다. (노드가 대신 할 예정)

        // 2. 바닥 체크 등 물리 계산은 그대로 유지
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 3. 애니메이션 업데이트는 그대로 유지
        UpdateAnimations();
    }

    public void SetInput(float direction)
    {
        horizontalInput = direction;

        // 방향 전환 로직 (Flip)
        if (horizontalInput > 0 && !facingRight) Flip();
        else if (horizontalInput < 0 && facingRight) Flip();
    }

    // 노드가 "점프해!"라고 명령할 함수
    public void DoJump()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Move the character
        Move();
        horizontalInput = 0;
    }
    
    private void Move()
    {
        // Apply horizontal movement
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }
    
    private void Jump()
    {
        // Reset vertical velocity and apply jump force
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    
    private void Flip()
    {
        // Toggle facing direction
        facingRight = !facingRight;
        
        // Flip the sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    private void UpdateAnimations()
    {
        // Priority: Jump > Run > Idle/Blink
        if (!isGrounded)
        {
            // Play jump animation
            animator.Play(ANIM_JUMP);
            isBlinking = false; // 점프 중에는 Blink 취소
        }
        else if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            // Play run animation
            animator.Play(ANIM_RUN);
            isBlinking = false; // 달리는 중에는 Blink 취소
        }
        else
        {
            // Idle state - check for blink
            if (!isBlinking && Time.time >= nextBlinkTime)
            {
                StartBlink();
            }
            
            if (isBlinking)
            {
                animator.Play(ANIM_BLINK);
            }
            else
            {
                animator.Play(ANIM_IDLE);
            }
        }
    }
    
    private void StartBlink()
    {
        isBlinking = true;
        Invoke(nameof(EndBlink), blinkDuration);
    }
    
    private void EndBlink()
    {
        isBlinking = false;
        SetNextBlinkTime();
    }
    
    private void SetNextBlinkTime()
    {
        nextBlinkTime = Time.time + Random.Range(minBlinkInterval, maxBlinkInterval);
    }
    
    // Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
