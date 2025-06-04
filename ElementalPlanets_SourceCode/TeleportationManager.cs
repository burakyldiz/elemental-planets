using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationManager : MonoBehaviour
{
    public static TeleportationManager Instance;

    [Header("Planet Scene Names")]
    public string[] planetSceneNames; // List of scene names for each planet

    [Header("Transition Effects")]
    public GameObject transitionEffectPrefab; // Optional transition effect

    private bool isTeleporting = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void TeleportToPlanet(string sceneName)
    {
        if (isTeleporting || string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Invalid teleportation request.");
            return;
        }

        StartCoroutine(TeleportRoutine(sceneName));
    }

    private System.Collections.IEnumerator TeleportRoutine(string sceneName)
    {
        isTeleporting = true;

        // Optional: Play transition effect
        if (transitionEffectPrefab != null)
        {
            Instantiate(transitionEffectPrefab, Vector3.zero, Quaternion.identity);
        }

        yield return new WaitForSeconds(1.0f); // Wait for the transition effect

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isTeleporting = false;
    }
}
