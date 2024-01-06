using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemStack : MonoBehaviour //, IPointerClickHandler
{
    public Item Item;

    public int Count {
        get { return _count; }
        set {
            if (value == 0 || value == 1) {
                Text.text = "";
                _count = value;
            }
            else {
                _count = value;
                Text.text = _count.ToString();
            }
        }
    }

    [SerializeField]
    private int _count;

    public Transform Slot;

    private TMPro.TextMeshProUGUI Text;
    private Image Sprite;

    private Item NoItem;

    private Player Player;

    private MouseStack MouseHeldItem;

    public void SetItem(Item NewItem) {
        if (NewItem == null) NewItem = NoItem;
        Item = NewItem;
        Sprite.sprite = NewItem.Image;
    }

    // Start is called before the first frame update
    void Awake()
    {
        NoItem = Resources.Load("NoItem") as Item;
        MouseHeldItem = FindObjectOfType<MouseStack>();
        Sprite = Slot.GetChild(0).GetComponent<Image>();
        //Sprite.sprite = spr;
        Text = Slot.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        Count = _count;
        SetItem(Item);
        Player = FindObjectOfType<Player>();
    }

    public bool RemoveItems(int Amount) {
        if (Count == -1)
            Amount = 0;

        if (Count - Amount < 0)
            return false;
        if (Count - Amount == 0) {
            ItemNull();
            return true;
        }

        Count -= Amount;
        return true;
    }

    public void PickUpItem() { 
        Item MouseItem = MouseHeldItem.Item;
        //int MouseCount = MouseHeldItem.Count;

        MouseHeldItem.SetItem(Item);
        //MouseHeldItem.Count = Count;

        SetItem(MouseItem);
        //Count = MouseCount;

        (MouseHeldItem.Count, Count) = (Count, MouseHeldItem.Count);

        Player.CC.RefreshRecipies();

        /*
        else if (Data.button == PointerEventData.InputButton.Right) {
            if (MouseHeldItem.Count != 0 || Count != 0) return;

            if (Count == 0 && MouseHeldItem.RemoveItems(1)) {
                Item = MouseHeldItem.Item;
                Count ++;
            }
            else if (MouseHeldItem.Count == 0 && RemoveItems(1)) {
                MouseHeldItem.Item = Item;
                MouseHeldItem.Count ++;
            }
        }
        */
    }

    public void ItemNull() {
        Item = NoItem;
        Sprite.sprite = NoItem.Image;
        Count = 0;
    }
}
