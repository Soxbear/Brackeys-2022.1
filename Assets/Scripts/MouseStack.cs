using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseStack : MonoBehaviour
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

    private TMPro.TextMeshProUGUI Text;
    private Image Sprite;

    public void SetItem(Item NewItem) {
        Sprite.sprite = NewItem.Image;
        Item = NewItem;
    }

    void Start() {
        Sprite = transform.GetChild(0).GetComponent<Image>();
        Text = transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        Count = _count;
    }

    void Update() {
        transform.position = Input.mousePosition;
    }
}
