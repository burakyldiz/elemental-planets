using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string itemName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{itemName} collected!");
            QuestManager.Instance?.UpdateQuestProgress(false, true); // Notify that an item is collected
            Destroy(gameObject); // Destroy the collectible after collection
        }
    }
}
