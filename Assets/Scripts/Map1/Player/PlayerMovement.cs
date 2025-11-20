using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float acceleration = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 6f;
    public float gravityMultiplier = 2f;

    //[Header("Ground Check")]
    //public Transform groundCheck;
    //public float groundRadius = 0.25f;
    //public LayerMask groundLayer;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.4f;

    private bool isDashing = false;
    private bool dashOnCooldown = false;

    private AudioSource audioSource;
    public AudioClip dashSound;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        if (dashSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        //if (Input.GetButtonDown("Jump"))
        //    jumpPressed = true;

        //if (groundCheck != null)
        //    isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        // ------ DASH INPUT ------
        if (!isDashing && !dashOnCooldown)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(1)) // A button or Right Mouse
            {
                StartCoroutine(Dash());
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            // --- Move ---
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            Vector3 targetVelocity = moveDir * moveSpeed;
            Vector3 velocity = rb.velocity;

            Vector3 velocityChange = (targetVelocity - new Vector3(velocity.x, 0, velocity.z))
                                    * acceleration * Time.fixedDeltaTime;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            //// --- Jump ---
            //if (jumpPressed && isGrounded)
            //{
            //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //    rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            //}
            //jumpPressed = false;

            // --- Extra gravity ---
            if (!isGrounded)
                rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
        }
    }
    //  DASH LOGIC
    IEnumerator Dash()
    {
        isDashing = true;
        dashOnCooldown = true;

        // Play dash sound
        if (dashSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dashSound);
        }

        // Determine dash direction
        Vector3 dashDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (dashDir.magnitude < 0.1f)
            dashDir = transform.forward;  // dash forward if no input

        dashDir.Normalize();

        // Disable gravity
        rb.useGravity = false;

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.velocity = dashDir * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        // Re-enable gravity + stop dash
        rb.useGravity = true;
        isDashing = false;

        // Small delay before next dash allowed
        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }
}
