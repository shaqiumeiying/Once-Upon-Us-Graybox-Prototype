using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyReaction : MonoBehaviour
{
    public enum ReactionType { None, FlashOnly, Knockback }

    [Header("Reaction Settings")]
    public ReactionType reactionType = ReactionType.Knockback;
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;
    public float knockbackForce = 4f;
    public AudioClip hitSound;

    private Renderer rend;
    private Color originalColor;
    private AudioSource audioSource;
    private Rigidbody rb;
    private bool flashing;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        originalColor = rend.material.color;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void ReactToHit(Vector3 hitDirection)
    {
        if (!flashing)
            StartCoroutine(FlashColor());

        if (hitSound)
            audioSource.PlayOneShot(hitSound);

        if (reactionType == ReactionType.Knockback)
        {
            // Start from the incoming hit direction
            Vector3 bounceDir = hitDirection.normalized;

            // Add an upward component
            bounceDir.y = 0f;

            // Normalize again to keep total force consistent
            bounceDir.Normalize();

            // Apply the knockback
            rb.AddForce(bounceDir * knockbackForce, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Vector3 hitDir = transform.position - collision.transform.position;
            ReactToHit(hitDir);
        }
    }


    private IEnumerator FlashColor()
    {
        flashing = true;
        rend.material.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
        flashing = false;
    }
}
