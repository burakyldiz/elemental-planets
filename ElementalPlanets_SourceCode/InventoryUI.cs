using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("UI References")]
    public GameObject inventoryPanel; // Reference to the Inventory Panel
    public GameObject itemTextPrefab; // Prefab for displaying items
    public Transform contentTransform; // Content Transform from the Scroll View

    private bool isInventoryVisible = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        
        RunSetupChecks();

       
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        
        TestPrefabInstantiation();
    }

    private void Update()
    {
        // Toggle inventory visibility with the I key
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryVisible = !isInventoryVisible;

            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(isInventoryVisible);
            }

            if (isInventoryVisible)
            {
                UpdateInventoryDisplay();
            }
        }
    }

    void UpdateInventoryDisplay()
    {
        // Ensure references are valid
        if (contentTransform == null)
        {
            Debug.LogError("Content Transform is null in InventoryUI. Ensure it is assigned.");
            return;
        }

        if (itemTextPrefab == null)
        {
            Debug.LogError("Item Text Prefab is null in InventoryUI. Ensure it is assigned.");
            return;
        }

        // Clear previous items
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // Populate inventory items
        foreach (string item in InventoryManager.Instance.playerInventory)
        {
            GameObject newItemText = Instantiate(itemTextPrefab, contentTransform);
            newItemText.SetActive(true);

            TextMeshProUGUI textComponent = newItemText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = item;
                Debug.Log($"Item '{item}' added to UI.");
            }
            else
            {
                Debug.LogError("Item Text Prefab does not have a TextMeshProUGUI component.");
            }
        }
    }

    private void TestPrefabInstantiation()
    {
        // Test instantiating the prefab to confirm it works
        if (itemTextPrefab != null && contentTransform != null)
        {
            GameObject testItem = Instantiate(itemTextPrefab, contentTransform);
            testItem.SetActive(true);

            TextMeshProUGUI textComponent = testItem.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "Test Item";
                Debug.Log("Test item instantiated successfully.");
            }
            else
            {
                Debug.LogError("Item Text Prefab does not have a TextMeshProUGUI component.");
            }
        }
        else
        {
            Debug.LogError("Test failed: Either Content Transform or Item Text Prefab is null.");
        }
    }

    private void RunSetupChecks()
    {
        // Check if key references are assigned
        if (inventoryPanel == null)
        {
            Debug.LogError("Inventory Panel is not assigned in InventoryUI.");
        }

        if (contentTransform == null)
        {
            Debug.LogError("Content Transform is not assigned in InventoryUI.");
        }

        if (itemTextPrefab == null)
        {
            Debug.LogError("Item Text Prefab is not assigned in InventoryUI.");
        }
    }
}
