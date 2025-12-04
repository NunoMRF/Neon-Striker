using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    [Header("Salto")]
    public float jumpHeight = 1.8f;
    public float gravity = -9.81f;

    [Header("Jump Responsivo (Coyote + Buffer)")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;
    private float coyoteTimer;
    private float jumpBufferTimer;

    private CharacterController controller;
    private Vector3 velocity;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    [Header("Animação")]
    [SerializeField] private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleInput();
        Movement();
        TryJump();
        ApplyGravity();
        DashLogic();
    }

    // -------------------------------------------------------------
    // INPUTS
    // -------------------------------------------------------------
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBufferTime;

        if (jumpBufferTimer > 0)
            jumpBufferTimer -= Time.deltaTime;

        // dash input
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldownTimer <= 0 && !isDashing)
            StartDash();
    }

    // -------------------------------------------------------------
    // MOVIMENTO BASE
    // -------------------------------------------------------------
    void Movement()
    {
        if (isDashing) return; // dash controla o movimento

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        
        
        bool hasInput = (new Vector2(x, z).sqrMagnitude > 0.001f);



        bool isRunning = hasInput && Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
                controller.Move(move * currentSpeed * Time.deltaTime);

        if (anim)
        {            float inputSpeed = new Vector2(x, z).magnitude;
            anim.SetFloat("Speed", inputSpeed);
            anim.SetBool("IsRunning", isRunning);
        }
    }

    // -------------------------------------------------------------
    // SALTO (Coyote Time + Buffer)
    // -------------------------------------------------------------
    void TryJump()
    {
        if (controller.isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            jumpBufferTimer = 0;
            coyoteTimer = 0;

            if (anim)
                anim.SetTrigger("Jump");
        }
    }

    // -------------------------------------------------------------
    // GRAVIDADE
    // -------------------------------------------------------------
    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (!isDashing)
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // -------------------------------------------------------------
    // DASH
    // -------------------------------------------------------------
    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        dashDirection = transform.forward; // dash sempre para a direção onde olha

        if (anim)
            anim.SetTrigger("Dash");
    }

    void DashLogic()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (!isDashing)
            return;

        controller.Move(dashDirection * dashForce * Time.deltaTime);

        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            isDashing = false;
        }
    }
}
