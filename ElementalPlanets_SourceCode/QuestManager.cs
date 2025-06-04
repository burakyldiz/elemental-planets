using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public enum QuestState { NotStarted, InProgress, Completed }

    [Header("Quest Objectives")]
    public int[] requiredEnemies; // Enemies required for each quest
    public int[] requiredItems;   // Items required for each quest

    private int currentEnemyProgress = 0;
    private int currentItemProgress = 0;

    [Header("UI References")]
    public GameObject questPanel;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public GameObject progressTracker;
    public TextMeshProUGUI questProgress;
    public TextMeshProUGUI messageDisplay; // Displays post-quest messages

    [Header("Quest States")]
    public QuestState[] questStates;
    public GameObject[] questObjects; // Quest-related objects (collectibles, boss, etc.)
    public int currentQuestIndex = 0;
    public bool questActive = false;

    [Header("Teleportation Pad")]
    public GameObject teleportationPad;

    private float messageDuration = 3f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeQuests();
    }

    void InitializeQuests()
    {
        questStates = new QuestState[requiredEnemies.Length];

        // Deactivate all quest-related objects initially
        for (int i = 0; i < questObjects.Length; i++)
        {
            if (questObjects[i] != null)
            {
                questObjects[i].SetActive(false);
            }
        }

        // Hide teleportation pad initially
        if (teleportationPad != null)
        {
            teleportationPad.SetActive(false);
        }
    }

    public void ShowQuestUI(string title, string description)
    {
        questPanel.SetActive(true);
        questTitle.text = title;
        questDescription.text = description;
    }

    public void HideQuestUI()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }

    public void ShowMessage(string message)
    {
        if (messageDisplay != null)
        {
            messageDisplay.text = message;
            messageDisplay.gameObject.SetActive(true);
            Invoke(nameof(HideMessage), messageDuration);
        }
    }

    private void HideMessage()
    {
        if (messageDisplay != null)
        {
            messageDisplay.gameObject.SetActive(false);
        }
    }

    public void StartQuest()
    {
        if (!questActive && currentQuestIndex < requiredEnemies.Length)
        {
            questActive = true;
            currentEnemyProgress = 0;
            currentItemProgress = 0;

            questStates[currentQuestIndex] = QuestState.InProgress;

            // Activate objects related to the current quest
            if (questObjects[currentQuestIndex] != null)
            {
                questObjects[currentQuestIndex].SetActive(true);

                // Activate Earth Guardian when starting the third quest
                EarthGuardian boss = questObjects[currentQuestIndex].GetComponent<EarthGuardian>();
                if (currentQuestIndex == 2 && boss != null)
                {
                    boss.ActivateBoss();
                }
            }

            progressTracker.SetActive(true);
            UpdateProgress();
            Debug.Log($"Quest {currentQuestIndex + 1} started!");
        }
    }

    public void UpdateQuestProgress(bool isEnemyDefeated, bool isItemCollected)
    {
        if (questActive)
        {
            if (isEnemyDefeated) currentEnemyProgress++;
            if (isItemCollected) currentItemProgress++;

            UpdateProgress();

            bool enemiesMet = currentEnemyProgress >= requiredEnemies[currentQuestIndex];
            bool itemsMet = currentItemProgress >= requiredItems[currentQuestIndex];

            if (enemiesMet && itemsMet)
            {
                CompleteQuest();
            }
        }
    }

    private void UpdateProgress()
    {
        string progress = "";

        if (requiredEnemies[currentQuestIndex] > 0)
        {
            progress += $"Enemies: {currentEnemyProgress}/{requiredEnemies[currentQuestIndex]}";
        }

        if (requiredItems[currentQuestIndex] > 0)
        {
            if (progress.Length > 0) progress += ", ";
            progress += $"Items: {currentItemProgress}/{requiredItems[currentQuestIndex]}";
        }

        questProgress.text = progress.Length > 0 ? progress : "No objectives for this quest.";
    }

   public void CompleteQuest()
{
    if (questActive)
    {
        bool enemiesMet = currentEnemyProgress >= requiredEnemies[currentQuestIndex];
        bool itemsMet = currentItemProgress >= requiredItems[currentQuestIndex];

        if (enemiesMet && itemsMet)
        {
            questActive = false;
            questStates[currentQuestIndex] = QuestState.Completed;
            Debug.Log($"Quest {currentQuestIndex + 1} completed!");
            progressTracker.SetActive(false);

            // Display post-quest message
            string postQuestMessage = NPCInteraction.Instance?.postQuestMessages[currentQuestIndex];
            if (!string.IsNullOrEmpty(postQuestMessage))
            {
                ShowMessage(postQuestMessage);
            }

            // Deactivate objects related to the completed quest
            if (questObjects[currentQuestIndex] != null)
            {
                questObjects[currentQuestIndex].SetActive(false);
            }

            // Activate teleportation pad for the final quest
            if (teleportationPad != null && currentQuestIndex == 2)
            {
                teleportationPad.SetActive(true);
                Debug.Log("Teleportation pad activated!");
            }

            currentQuestIndex++;
        }
    }
}



    public QuestState GetQuestState(int questIndex)
    {
        return questIndex >= 0 && questIndex < questStates.Length ? questStates[questIndex] : QuestState.NotStarted;
    }
}
