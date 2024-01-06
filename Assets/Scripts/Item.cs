using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject {
    public string Name;
    public string Description;
    public Sprite Image;
    public bool Stackable;
    public ItemAction ItemAction;
    public string ToolAnim;
    public GameObject PlacedObject;
    public List<int> ExtraParams;
}

public class ItemUses {
    public ItemUse GetUse(string Name) {
        return Resources.Load("ItemUses/" + Name) as ItemUse;
    }
}

public enum ItemAction {
    Resource,
    Consume,
    Harvest,
    Placeable
}


public interface ItemUse {
    //public UnityEvent UseEvent{ get; set; }
    public void Use(List<int> ExtraParams);
}

[System.Serializable]
public struct CraftingItem {
    public Item Item;
    public int Amount;

    public CraftingItem(Item _Item, int _Amount = 1) {
        Item = _Item;
        Amount = _Amount;
    }
}