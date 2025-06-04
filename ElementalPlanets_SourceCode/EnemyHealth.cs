using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Rigidbody rb;
    private Animator animator;
    private bool isDying = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
            Debug.Log($"{gameObject.name} takes {damage} damage. Current health: {currentHealth}");
        }
        else
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector3 knockbackForce)
    {
        if (rb != null)
        {
            rb.AddForce(knockbackForce, ForceMode.Impulse);
        }
    }

    private void Die()
    {
        if (isDying) return;
        isDying = true;

        Debug.Log($"{gameObject.name} has been defeated!");

        // Trigger Die animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Notify QuestManager if this is the EarthGuardian
        if (gameObject.CompareTag("EarthGuardian"))
        {
            QuestManager.Instance?.UpdateQuestProgress(true, false);
        }

        // Delay destruction to allow animation to play
        Destroy(gameObject, 1f); 
    }
}
