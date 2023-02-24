using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory System/Recipe")]
public class Recipe : ScriptableObject
{
    public string name;
    public InventoryItemData result;
    public InventoryItemData[] ingredients;
    public bool IsMatch(InventoryItemData[] items)
    {
        // Make sure the number of items matches the number of ingredients in the recipe
        if (items.Length != ingredients.Length)
        {
            return false;
        }

        // Check each item against its corresponding ingredient in the recipe
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != ingredients[i])
            {
                return false;
            }
        }

        // If all items match their corresponding ingredients, the combination is a match
        return true;
    }

}
