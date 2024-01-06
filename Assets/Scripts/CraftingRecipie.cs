using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipie", menuName = "Items/Crafting Recipie")]
public class CraftingRecipie : ScriptableObject {
    public Item Result;
    [Header("4 ingridients max")]
    public CraftingItem[] Ingridients;
}