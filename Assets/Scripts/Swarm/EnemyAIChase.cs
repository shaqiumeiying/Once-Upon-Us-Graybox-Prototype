using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAIChase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.1f;

    private Transform player;
    private Rigidbody rb;
    private bool playerInRange = false;
    private bool recoveringFromHit = false;
    private Animator anim;

    [Header("Obstacle Avoidance")]
    public float avoidStrength = 5f;
    public float avoidDistance = 1f;

    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // IMPORTANT ¡ú prevents clipping!
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (playerInRange && player != null && !recoveringFromHit)
        {
            ChasePlayer();
            FlipTowardsPlayer();
        }
    }

    void ChasePlayer()
    {
        Vector3 toPlayer = (player.position - transform.position);
        toPlayer.y = 0;

        Vector3 desiredDir = toPlayer.normalized;

        // SAFER wall handling
        Vector3 finalDir = AvoidWalls(desiredDir);

        if (toPlayer.magnitude > stoppingDistance)
        {
            Vector3 moveStep = finalDir * moveSpeed * Time.deltaTime;

            // The correct way to move a Rigidbody without phasing through walls
            rb.MovePosition(rb.position + moveStep);
        }
        else
        {
            // Stop movement
            rb.velocity = Vector3.zero;
        }
    }

    Vector3 AvoidWalls(Vector3 desiredDir)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, desiredDir, out hit, avoidDistance))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                // Horizontal-only slide direction based on wall normal
                Vector3 wallNormal = hit.normal;

                // Perpendicular to wall, but force Y=0
                Vector3 slide = Vector3.Cross(wallNormal, Vector3.up);
                slide.y = 0;
                slide.Normalize();

                // Return the stronger horizontal direction
                return Mathf.Abs(slide.x) > Mathf.Abs(slide.z)
                    ? new Vector3(slide.x, 0, 0)
                    : new Vector3(0, 0, slide.z);
            }
        }

        return desiredDir;
    }


    void FlipTowardsPlayer()
    {
        if (!player) return;

        if (player.position.x > transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    public void SetPlayerInRange(Transform playerTransform)
    {
        player = playerTransform;
        playerInRange = true;

        if (anim)
            anim.SetTrigger("Detected");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Player"))
        {
            StartCoroutine(HandleHit(col));
        }
    }

    IEnumerator HandleHit(Collision col)
    {
        recoveringFromHit = true;

        EnemyReaction react = GetComponent<EnemyReaction>();
        if (react != null)
        {
            Vector3 hitDir = transform.position - col.transform.position;
            react.ReactToHit(hitDir);
        }

        yield return new WaitForSeconds(0.15f);
        recoveringFromHit = false;
    }
}
