using UnityEngine;

public class EarthGuardian : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 500f;
    private float currentHealth;

    [Header("Attack Settings")]
    public float groundSlamDamage = 50f;
    public float shockwaveRadius = 5f;
    public float slamCooldown = 5f;
    public ParticleSystem shockwaveEffect;

    public GameObject boulderPrefab;
    public Transform throwOrigin;
    public float throwForce = 15f;
    public float throwCooldown = 7f;

    [Header("Animator")]
    private Animator animator;

    private float lastSlamTime = -Mathf.Infinity;
    private float lastThrowTime = -Mathf.Infinity;

    private bool isDying = false;
    private bool isQuestActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDying || !isQuestActive) return;

        // Check cooldowns and trigger attacks
        if (Time.time >= lastSlamTime + slamCooldown)
        {
            PerformGroundSlam();
            lastSlamTime = Time.time;
        }

        if (Time.time >= lastThrowTime + throwCooldown)
        {
            ThrowBoulder();
            lastThrowTime = Time.time;
        }
    }

    public void ActivateBoss()
    {
        isQuestActive = true;
        Debug.Log("Earth Guardian activated and ready to attack!");
    }

    public void TakeDamage(float damage)
    {
        if (isDying) return;

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("TakeDamage");
            }
            Debug.Log($"Earth Guardian takes {damage} damage. Current health: {currentHealth}");
        }
        else
        {
            Die();
        }
    }

    private void PerformGroundSlam()
    {
        Debug.Log("PerformGroundSlam Triggered!");

        if (animator != null)
        {
            animator.SetTrigger("GroundSlam");
        }

        if (shockwaveEffect != null)
        {
            shockwaveEffect.Play();
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                playerHealth?.TakeDamage(groundSlamDamage);
            }
        }
    }

    private void ThrowBoulder()
    {
        Debug.Log("ThrowBoulder Triggered!");

        if (animator != null)
        {
            animator.SetTrigger("ThrowBoulder");
        }

        Invoke(nameof(SpawnBoulder), 0.5f);
    }

    private void SpawnBoulder()
    {
        if (boulderPrefab != null && throwOrigin != null)
        {
            GameObject boulder = Instantiate(boulderPrefab, throwOrigin.position, Quaternion.identity);
            Rigidbody rb = boulder.GetComponent<Rigidbody>();
            if (rb != null)
            {
                PlayerHealth player = FindObjectOfType<PlayerHealth>();
                if (player != null)
                {
                    Vector3 direction = (player.transform.position - throwOrigin.position).normalized;
                    rb.AddForce(direction * throwForce, ForceMode.Impulse);
                }
            }
        }
    }

    private void Die()
{
    if (isDying) return;
    isDying = true;

    Debug.Log("Earth Guardian defeated!");
    if (animator != null)
    {
        animator.SetTrigger("Die");
    }

    // Notify the QuestManager that the Earth Guardian is defeated
    QuestManager.Instance?.UpdateQuestProgress(true, false);

    // Destroy the GameObject after the animation plays
    Destroy(gameObject, 2f); 
}

}
