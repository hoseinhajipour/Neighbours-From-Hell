using System.Collections.Generic;
using UnityEngine;

public class Commode : MonoBehaviour
{
    public List<InventoryItemData> itemsToAdd;
    private bool hasBeenUsed = false; // flag to indicate if the itemsToAdd list has been used
    private Collider other;
    public GameObject textInfo;
    private void Update()
    {
        if (other)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (other.CompareTag("Player"))
                {

                    if (!hasBeenUsed)
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
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInfo.SetActive(true);
            Vector3 direction = Camera.main.transform.position - textInfo.transform.position;

            // Calculate the rotation that points in the opposite direction
            Quaternion oppositeRotation = Quaternion.LookRotation(-direction, Vector3.up);
            // Rotate the textInfo to face away from the camera and towards the player
            textInfo.transform.rotation = oppositeRotation;
            
            this.other = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInfo.SetActive(false);
            this.other = null;
        }
    }

}
