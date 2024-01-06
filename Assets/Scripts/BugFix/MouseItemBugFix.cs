using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseItemBugFix : MonoBehaviour
{
    ItemStack Stack;
    private Item NoItem;
    void Start() {
        Stack = GetComponent<ItemStack>();
        NoItem = Resources.Load("NoItem") as Item;
        Debug.Log(NoItem);
    }
    void LateUpdate()
    {
        if (Stack.Item.name == NoItem.name) {
            Stack.Count = 0;
        }
    }
}
