using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingIngridientDisplay : MonoBehaviour
{
    private Image Sprite;
    private TextMeshProUGUI Text;

    private Sprite NoSprite;

    void Awake()
    {
        Sprite = GetComponent<Image>();
        Text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        NoSprite = FindObjectOfType<CraftingController>().NoSprite;
    }

    public void UpdateRequirements(CraftingItem New) {
        Sprite.sprite = New.Item.Image;

        if (New.Amount == 0)
            Text.text = "";
        else
            Text.text = New.Amount.ToString();
    }

    public void NoItem() {
        Text.text = "";
        Sprite.sprite = NoSprite;
    }
}
