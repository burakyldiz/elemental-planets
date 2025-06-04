using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDisplayManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillUI
    {
        public Image skillIcon; // The skill icon image
        public Image cooldownOverlay; // The cooldown overlay
        public TextMeshProUGUI cooldownText; // The cooldown text
        public string key; // The key to use the skill
    }

    public SkillUI[] skills; // Array to store skill UI components

    private float[] cooldownDurations = { 2f, 5f }; // Example cooldowns for skills (Skill1, Skill2)
    private float[] currentCooldowns; // Tracks the current cooldown time for each skill

    void Start()
    {
        // Initialize cooldowns
        currentCooldowns = new float[skills.Length];

        // Initialize UI elements for each skill
        for (int i = 0; i < skills.Length; i++)
        {
            SetCooldownVisibility(i, false); // Hide cooldown overlay and text initially
        }
    }

    void Update()
{
    // Update cooldowns for each skill
    for (int i = 0; i < skills.Length; i++)
    {
        if (currentCooldowns[i] > 0)
        {
            // Reduce cooldown over time
            currentCooldowns[i] -= Time.deltaTime;

            // Update the cooldown overlay and text
            skills[i].cooldownOverlay.fillAmount = currentCooldowns[i] / cooldownDurations[i];
            skills[i].cooldownText.text = Mathf.Ceil(currentCooldowns[i]).ToString();

            // Ensure the overlay and text are visible
            SetCooldownVisibility(i, true);
        }
        else
        {
            // Ensure cooldown visuals are reset when cooldown reaches 0
            if (currentCooldowns[i] <= 0)
            {
                currentCooldowns[i] = 0; // Clamp cooldown timer to 0
                skills[i].cooldownOverlay.fillAmount = 0; // Hide overlay
                skills[i].cooldownText.text = ""; // Clear cooldown text
                SetCooldownVisibility(i, false); // Ensure overlay and text are hidden
                // Debug.Log($"Skill {i + 1} cooldown ended and visuals reset!");
            }
        }
    }

    // Check for skill usage
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        UseSkill(0); // Use skill 1
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
        UseSkill(1); // Use skill 2
    }
}


    public void UseSkill(int skillIndex)
    {
        // Check if the skill is on cooldown
        if (currentCooldowns[skillIndex] > 0)
        {
           // Debug.Log($"Skill {skillIndex + 1} is on cooldown! Time remaining: {currentCooldowns[skillIndex]:F1} seconds.");
            return; // Prevent skill usage
        }

        // Skill is ready to use
        //Debug.Log($"Skill {skillIndex + 1} used!");
        currentCooldowns[skillIndex] = cooldownDurations[skillIndex]; // Start cooldown

        // Add your skill-specific logic here (e.g., animations, effects)
    }

    private void SetCooldownVisibility(int skillIndex, bool isVisible)
    {
        // Set the visibility of the cooldown overlay and text
        skills[skillIndex].cooldownOverlay.gameObject.SetActive(isVisible);
        skills[skillIndex].cooldownText.gameObject.SetActive(isVisible);
    }
}
