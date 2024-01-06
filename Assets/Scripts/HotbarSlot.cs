using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlot : MonoBehaviour
{
    public int SlotId;
    Image SlotSprite;
    Image Sprite;
    TextMeshProUGUI Text;

    public ItemStack SlotToCopy;

    void Start() {
        Sprite = transform.GetChild(0).GetComponent<Image>();
        Text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate() {
        SlotSprite = GetComponent<Image>();
        Sprite.sprite = SlotToCopy.Item.Image;
        if (SlotToCopy.Count == 0 || SlotToCopy.Count == 1)
            Text.text = "";
        else
            Text.text = SlotToCopy.Count.ToString();
    }

    public void SwapMainPlayerItem(int Slot) {

        if (SlotId == Slot)
            SlotSprite.color = new Color(1, 0.5f, 0);
        else
            SlotSprite.color = new Color(1, 1, 1);
    }
}
