using UnityEngine;

public class TeleportationPad : MonoBehaviour
{
    [Header("Target Scene")]
    public string targetSceneName; // Name of the scene to load

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers teleportation
        {
            Debug.Log($"Teleporting to {targetSceneName}");
            TeleportationManager.Instance?.TeleportToPlanet(targetSceneName);
        }
    }
}
