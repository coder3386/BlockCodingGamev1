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
    private float horizontalInput; // 외부에서 값을 넣어줄 변수
    private bool jumpRequested = false; // 점프 신호 확인용
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
    private float blinkDuration = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.parent = transform;
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObj.transform;
        }

        SetNextBlinkTime();
    }

    void Update()
    {
        // ★ [수정 1] Input.GetAxisRaw 제거!
        // 이제 horizontalInput은 아래의 SetMoveInput 함수가 바꿔줍니다.

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ★ [수정 2] Input.GetButtonDown 제거!
        // 대신 외부에서 DoJump()를 호출해서 jumpRequested가 true가 되면 점프합니다.
        if (jumpRequested && isGrounded)
        {
            Jump();
            jumpRequested = false; // 점프했으니 신호 끄기
        }

        // Handle sprite flipping
        if (horizontalInput > 0 && !facingRight) Flip();
        else if (horizontalInput < 0 && facingRight) Flip();

        // Update animations
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        Move();

        // ★ [핵심] 한 번 움직였으면 입력을 0으로 초기화
        // 노드(NodeRunner)가 계속 값을 주지 않으면 캐릭터는 멈춰야 합니다.
        horizontalInput = 0f;
        jumpRequested = false; // (안전장치) 점프 신호도 초기화
    }

    private void Move()
    {
        // Apply horizontal movement
        // (Unity 6 최신 버전 호환을 위해 linearVelocity 사용)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.Play(ANIM_JUMP); // 점프 즉시 애니메이션 재생
        isBlinking = false;
    }

    // =========================================================
    // ★ [외부(노드)에서 호출할 함수들]
    // =========================================================

    // 1. "이동해!" (NodeRunner가 호출)
    public void SetMoveInput(float direction)
    {
        horizontalInput = direction;
    }

    // 2. "점프해!" (NodeRunner가 호출)
    public void DoJump()
    {
        // 땅에 있을 때만 점프 신호를 받음
        if (isGrounded)
        {
            jumpRequested = true;
        }
    }

    // =========================================================
    // 아래는 기존 애니메이션 & 깜빡임 코드 그대로 유지
    // =========================================================

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void UpdateAnimations()
    {
        if (!isGrounded)
        {
            animator.Play(ANIM_JUMP);
            isBlinking = false;
        }
        else if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            animator.Play(ANIM_RUN);
            isBlinking = false;
        }
        else
        {
            if (!isBlinking && Time.time >= nextBlinkTime) StartBlink();

            if (isBlinking) animator.Play(ANIM_BLINK);
            else animator.Play(ANIM_IDLE);
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

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}