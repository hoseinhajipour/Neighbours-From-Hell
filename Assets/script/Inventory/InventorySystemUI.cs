using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystemUI : MonoBehaviour
{
    public GameObject itemPrefab; // Prefab of the UI element to show each inventory item
    public Transform contentPanel; // Panel where the inventory items will be shown


    private InventorySystem inventorySystem; // Reference to the InventorySystem

    private void Start()
    {
        inventorySystem = GameObject.Find("Inventory").GetComponent<InventorySystem>();
        if (inventorySystem != null)
        {
            inventorySystem.onInventoryChange += UpdateUI; // Subscribe to the onInventoryChange event of the InventorySystem
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        if (inventorySystem != null)
        {
            inventorySystem.onInventoryChange -= UpdateUI; // Unsubscribe from the onInventoryChange event of the InventorySystem
        }
    }

    private void UpdateUI()
    {
        // Remove all current inventory items in the UI
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }

        // Add all inventory items to the UI
        foreach (InventoryItem item in inventorySystem.inventory)
        {
            GameObject newItem = Instantiate(itemPrefab, contentPanel);
            newItem.transform.Find("Icon").GetComponent<Image>().sprite = item.data.icon;
            newItem.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.data.dispalyName;
            newItem.transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = "x" + item.stackSize.ToString();
        }
    }
}
