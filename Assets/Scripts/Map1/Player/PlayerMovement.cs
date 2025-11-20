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
    private bool isGrounded;

    private Animator anim;               // << ADD
    private SpriteRenderer sr;           // << ADD

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        anim = GetComponentInChildren<Animator>();   // << ADD
        sr = GetComponentInChildren<SpriteRenderer>(); // << ADD
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        UpdateAnimation();   // << ADD

        // ------ DASH INPUT ------
        if (!isDashing && !dashOnCooldown)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
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

            // --- Extra gravity ---
            if (!isGrounded)
                rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    // ---------------------------------------------------
    //  DASH
    // ---------------------------------------------------
    IEnumerator Dash()
    {
        isDashing = true;
        dashOnCooldown = true;

        if (dashSound != null)
            audioSource.PlayOneShot(dashSound);

        Vector3 dashDir = new Vector3(moveInput.x, 0, moveInput.y);
        if (dashDir.magnitude < 0.1f)
            dashDir = transform.forward;  // dash forward if no input

        dashDir.Normalize();

        rb.useGravity = false;

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.velocity = dashDir * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.useGravity = true;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }

    // ---------------------------------------------------
    //  ANIMATION UPDATE
    // ---------------------------------------------------
    void UpdateAnimation()
    {
        if (anim == null) return;

        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;

        anim.SetFloat("Speed", speed);   // walk/idle blend

        // Flip character horizontally (if needed)
        if (speed > 0.1f)
        {
            if (moveInput.x > 0.1f)
                sr.flipX = false;
            else if (moveInput.x < -0.1f)
                sr.flipX = true;
        }
    }
}
