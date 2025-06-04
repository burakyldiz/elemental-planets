using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public static NPCInteraction Instance;

    [Header("NPC Details")]
    public string npcName = "Elder Terra";
    public string[] questTitles; // Titles for each quest
    public string[] questDescriptions; // Descriptions for each quest
    public string[] postQuestMessages; // Post-quest messages for each quest

    [Header("UI References")]
    public GameObject interactionHint;
    private bool isPlayerInRange = false;

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
    }

    void Start()
    {
        if (interactionHint != null)
        {
            interactionHint.SetActive(false); // Start with the hint hidden
        }
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowQuestPanel();
            }
            else if (Input.GetKeyDown(KeyCode.F)) // Confirm the quest
            {
                ConfirmQuest();
            }
            else if (Input.GetKeyDown(KeyCode.X)) // Decline the quest
            {
                DeclineQuest();
            }
        }
    }

    void ShowQuestPanel()
    {
        int currentQuestIndex = QuestManager.Instance.currentQuestIndex;

        if (currentQuestIndex < questTitles.Length)
        {
            if (QuestManager.Instance.GetQuestState(currentQuestIndex) == QuestManager.QuestState.NotStarted)
            {
                QuestManager.Instance?.ShowQuestUI(questTitles[currentQuestIndex], questDescriptions[currentQuestIndex]);
            }
            else if (QuestManager.Instance.GetQuestState(currentQuestIndex) == QuestManager.QuestState.Completed)
            {
                QuestManager.Instance?.ShowMessage(postQuestMessages[currentQuestIndex]); // Show post-quest message
            }
        }
        else
        {
            Debug.Log("No more quests available.");
        }
    }

    void ConfirmQuest()
    {
        Debug.Log("Quest confirmed!");
        QuestManager.Instance?.StartQuest();
        QuestManager.Instance?.HideQuestUI();
        QuestManager.Instance?.ShowMessage(questDescriptions[QuestManager.Instance.currentQuestIndex]); // Show pre-quest message
    }

    void DeclineQuest()
    {
        Debug.Log("Quest declined!");
        QuestManager.Instance?.HideQuestUI();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionHint?.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionHint?.SetActive(false);
            QuestManager.Instance?.HideQuestUI();
        }
    }
}
