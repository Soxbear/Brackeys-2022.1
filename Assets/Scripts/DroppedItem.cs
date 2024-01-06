using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item Item;

    public void ChangeItem(Item item) {
        Item = item;
        GetComponent<SpriteRenderer>().sprite = item.Image;
    }

    void OnTriggerStay2D(Collider2D col) {
        bool PickUp = col.GetComponent<Player>().ItemPickUp(Item);
        if (PickUp) {
            Destroy(gameObject);
        }
    }
}
