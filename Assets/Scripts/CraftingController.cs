using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    private Player Player;
    public List<CraftingRecipie> Recipies;

    public GameObject RecipieBox;

    private ItemUtility ItemUtility;

    [HideInInspector]
    public CraftingRecipie Rec;

    List<CraftingButton> Crafts;

    private Transform RecipieHolder;

    public Sprite NoSprite;

    public void Craft() {
        if (Rec == null)
            return;

        Player.RemoveFromInventory(new List<CraftingItem>(Rec.Ingridients));
        if (!Player.ItemPickUp(Rec.Result)) {
            ItemUtility.DropItem(Player.transform.position, Rec.Result);
        }
        RefreshRecipies();
    }

    public void SelectCraftItem(CraftingRecipie Recipie) {
        Rec = Recipie;
    }

    public void RefreshRecipies() {
        foreach (CraftingButton craftingButton in Crafts) {
            craftingButton.Refresh();
        }
        foreach (ItemStack Stack in Player.InventoryItems) {
            foreach (CraftingButton craftingButton in Crafts) {
            craftingButton.Feed(Stack);
            }
        }
        foreach (CraftingButton craftingButton in Crafts) {
            craftingButton.Finish();
        }
    }

    void Awake() {
        Player = FindObjectOfType<Player>();
        ItemUtility = FindObjectOfType<ItemUtility>();
        RecipieHolder = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        Crafts = new List<CraftingButton>();
        foreach (CraftingRecipie recipie in Recipies) {
            GameObject Button = Instantiate(RecipieBox, RecipieHolder);
            Button.GetComponent<CraftingButton>().SetVars(recipie);
            Crafts.Add(Button.GetComponent<CraftingButton>());
        }
    }

    void OnEnable() {
        RefreshRecipies();
    }
}
