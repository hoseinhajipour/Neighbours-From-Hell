using UnityEngine;
[CreateAssetMenu(menuName = "Inventory System/Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public int id;
    public string dispalyName;
    public Sprite icon;
    public GameObject prefab;
}