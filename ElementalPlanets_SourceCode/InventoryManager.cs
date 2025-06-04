using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<string> playerInventory = new List<string>(); 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    public void AddItem(string item)
    {
        playerInventory.Add(item);
        Debug.Log($"Item added: {item}. Current Inventory Count: {playerInventory.Count}");
    }

    public void RemoveItem(string item)
    {
        if (playerInventory.Contains(item))
        {
            playerInventory.Remove(item);
            Debug.Log($"Item removed: {item}. Current Inventory Count: {playerInventory.Count}");
        }
    }

    public bool HasItem(string item)
    {
        return playerInventory.Contains(item);
    }
}
