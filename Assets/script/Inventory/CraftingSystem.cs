using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public Recipe[] recipes;

    public bool Craft(InventorySystem inventory, Recipe recipe)
    {
        // Check if player has all the ingredients for the recipe
        foreach (InventoryItemData ingredient in recipe.ingredients)
        {
            if (inventory.Get(ingredient) == null)
            {
                Debug.Log("Missing ingredient: " + ingredient.dispalyName);
                return false;
            }
        }

        // Remove ingredients from player's inventory
        foreach (InventoryItemData ingredient in recipe.ingredients)
        {
            inventory.Remove(ingredient);
        }

        // Add result to player's inventory
        inventory.Add(recipe.result);

        Debug.Log("Crafted: " + recipe.result.dispalyName);
        return true;
    }

    public bool Craft(InventorySystem inventory, string recipeName)
    {
        // Find recipe with the given name
        Recipe recipe = null;
        foreach (Recipe r in recipes)
        {
            if (r.name == recipeName)
            {
                recipe = r;
                break;
            }
        }

        // Craft the recipe if found
        if (recipe != null)
        {
            return Craft(inventory, recipe);
        }
        else
        {
            Debug.Log("Recipe not found: " + recipeName);
            return false;
        }
    }
}
