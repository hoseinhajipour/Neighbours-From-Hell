using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;

    public List<Recipe> recipes = new List<Recipe>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public bool IsCraftable(InventoryItemData[] ingredients, out InventoryItemData result)
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.IsMatch(ingredients))
            {
                result = recipe.result;
                return true;
            }
        }

        result = null;
        return false;
    }
}
