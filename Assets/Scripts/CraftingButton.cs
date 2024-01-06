using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
    private List<CraftingItem> Reqs;
    
    private CraftingRecipie Rec;

    private bool ReqFail;
    private int ReqsMet;

    private CraftingController Controller;
    void Awake()
    {
        Controller = transform.parent.parent.parent.parent.parent.GetComponent<CraftingController>();
    }

    public void SetVars(CraftingRecipie Recipie) {
        Reqs = new List<CraftingItem>(Recipie.Ingridients);
        Rec = Recipie;
        transform.GetChild(0).GetComponent<Image>().sprite = Recipie.Result.Image;
        for (int i = 0; i < 4; i++) {
            if (Recipie.Ingridients.Length > i) {
                transform.GetChild(i + 1).GetComponent<CraftingIngridientDisplay>().UpdateRequirements(Recipie.Ingridients[i]);
            }
            else
            transform.GetChild(i + 1).GetComponent<CraftingIngridientDisplay>().NoItem();
        }
    }

    public void Refresh() {
        ReqFail = false;
        ReqsMet = 0;
    }

    public void Feed(ItemStack Food) {
        foreach (CraftingItem Craft in Reqs) {
            if (Food.Item == Craft.Item) {
                ReqsMet ++;
                if (Food.Count < Craft.Amount)
                    ReqFail = true;
            }
        }
    }

    public void Finish() {
        if (ReqFail || ReqsMet < Reqs.Count) {
            GetComponent<Button>().interactable = false;
            if (Controller.Rec == Rec) {
                Controller.Rec = null;
            }
        }
        else
            GetComponent<Button>().interactable = true;
    }

    public void OnPress() {
        Controller.SelectCraftItem(Rec);
    }
}
