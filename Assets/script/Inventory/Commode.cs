using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commode : MonoBehaviour
{
    public List<InventoryItemData> itemsToAdd;
    private bool hasBeenUsed = false; // flag to indicate if the itemsToAdd list has been used

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenUsed && other.CompareTag("Player"))
        {

            InventorySystem inventorySystem = GameObject.Find("Inventory").GetComponent<InventorySystem>();
            if (inventorySystem != null)
            {
                foreach (InventoryItemData item in itemsToAdd)
                {
                    inventorySystem.Add(item);
                    Debug.Log(item.dispalyName + " Add to Inventory");
                }
                itemsToAdd.Clear(); // remove the itemsToAdd list to prevent adding the items again
                hasBeenUsed = true; // set the flag to indicate that the itemsToAdd list has been used


            }
        }
    }

}
