using UnityEngine;

public class RockDamage : MonoBehaviour
{
    public float damage = 100f;
    public float knockbackForce = 10f;
    public float destroyDelay = 2f;

    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Find the root GameObject with the EnemyHealth script
        EnemyHealth enemyHealth = collision.collider.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            // Apply damage
            enemyHealth.TakeDamage(damage);

            // Apply knockback if Rigidbody is present
            Rigidbody rb = collision.collider.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
                knockbackDirection.y = 1f; // Add upward force
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }

        // Destroy the rock
        Destroy(gameObject);
    }
}
