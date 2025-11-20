using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isInvincible = false;

    [Header("Hit Reaction")] 
    public float invincibleDuration = 2f;  
    public float flashInterval = 0.1f;

    [Header("Ouch Sounds")]
    public AudioClip ouchSound;
    private AudioSource audioSource;

    public UIHeartsController heartsUI;

    private Rigidbody rb;
    private Collider col;
    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isInvincible) return;


            if (collision.collider.CompareTag("Enemy"))
            {
                Debug.Log("Collided with Enemy");
                TakeDamage(1);
            }

    }



    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        audioSource.PlayOneShot(ouchSound);

        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        Debug.Log($"Player hit! HP: {currentHealth}/{maxHealth}");

        heartsUI.UpdateHearts(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
        else
            StartCoroutine(InvincibilityFlash());
    }
    
    IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float timer = 0f;

        while (timer < invincibleDuration)
        {
            if (sr)
                sr.enabled = !sr.enabled; // flashing

            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        if (sr)
            sr.enabled = true;

        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene("EnemySwarm");
    }

}
