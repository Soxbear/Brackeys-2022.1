using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUtility : MonoBehaviour
{
    public GameObject DroppedItemPreset;
    
    public void DropItem(Vector2 Pos, Item Item) {
        GameObject Drop = Instantiate(DroppedItemPreset, new Vector3(Pos.x, Pos.y), new Quaternion());
        Drop.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        Drop.GetComponent<DroppedItem>().ChangeItem(Item);
    }

    public void DropItem(Vector2 Pos, Item Item, float MinDist, float MaxDist = 1) {
        GameObject Drop = Instantiate(DroppedItemPreset, new Vector3(Pos.x, Pos.y), new Quaternion());

        Vector3 DropPos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

        DropPos = DropPos.normalized * Random.Range(MinDist, MaxDist);

        Drop.transform.position += DropPos;
        Drop.GetComponent<DroppedItem>().ChangeItem(Item);
    }

    public void DropItemSpot(Vector2 Pos, Item Item) {
        GameObject Drop = Instantiate(DroppedItemPreset, new Vector3(Pos.x, Pos.y), new Quaternion());
        Drop.GetComponent<DroppedItem>().ChangeItem(Item);
    }
}
