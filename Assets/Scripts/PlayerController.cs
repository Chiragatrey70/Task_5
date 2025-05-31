using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float crouchSpeed = 1f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    [Header("Camera")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;
    private bool isRunning;
    public Transform playerBody;       // Your main player object (root of Mixamo model)
    public Transform fppCamera;        // The FirstPersonCamera
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float speed = 5f; // if not already there
      // Vertical look rotation


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        if (Camera.main.name == "FirstPersonCamera")
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents looking too far up/down

            fppCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // vertical tilt
            playerBody.Rotate(Vector3.up * mouseX); // horizontal turn
        }

    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move;

    if (Camera.main.name == "FirstPersonCamera"){
    // Move relative to where the player is facing (first-person)
    move = playerBody.right * horizontal + playerBody.forward * vertical;
}
else
{
    // Move relative to the third-person camera
    move = cameraTransform.right * horizontal + cameraTransform.forward * vertical;
}
move.y = 0f;


        float currentSpeed = walkSpeed;
        isRunning = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            isRunning = true;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        controller.Move(move.normalized * currentSpeed * Time.deltaTime);

        // Animation parameters
        float animationSpeed = move.magnitude * currentSpeed;
        animator.SetFloat("Speed", animationSpeed);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsCrouching", isCrouching);

        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

if (inputDirection.magnitude >= 0.1f && Camera.main.name != "FirstPersonCamera")
{
    float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
    float angle = Mathf.SmoothDampAngle(playerBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    playerBody.rotation = Quaternion.Euler(0f, angle, 0f);

    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    CharacterController controller = GetComponent<CharacterController>();
    controller.Move(moveDir.normalized * speed * Time.deltaTime);
}

    }

    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }

        if (isGrounded && velocity.y <= 0)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
