using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    [Header("UI References")]
    public GameObject questPanel;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questProgress;

    void Awake()
    {
        // Ensure only one QuestUIManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowQuest(string title, string description)
    {
        questPanel.SetActive(true);
        questTitle.text = title;
        questDescription.text = description;
    }

    public void UpdateProgress(string progress)
    {
        questProgress.text = progress;
    }

    public void HideQuest()
    {
        questPanel.SetActive(false);
    }
}
