using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    [SerializeField] private Animator anim;   // arrastar no Inspector

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // INPUT
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        
        bool hasInput = (new Vector2(x, z).sqrMagnitude > 0.001f);

        // SHIFT para correr
        bool isRunning = hasInput && Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // MOVIMENTO
        controller.Move(move * currentSpeed * Time.deltaTime);

        // ANIMAÇÃO
        if (anim != null)
        {
            
            float inputSpeed = new Vector2(x, z).magnitude;

            anim.SetFloat("Speed", inputSpeed);
            anim.SetBool("IsRunning", isRunning);
        }

        // GRAVIDADE
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
