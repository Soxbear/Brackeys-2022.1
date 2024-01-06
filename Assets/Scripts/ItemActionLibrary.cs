using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionLibrary : MonoBehaviour
{
    public void DoAction(Item Item) {
        switch (Item.ItemAction) {
            case ItemAction.Harvest :
                Harvest(Item.ExtraParams[0], Item.ExtraParams[1], Item.ExtraParams[2], Item.ToolAnim);
                break;
            
            case ItemAction.Consume :
                Player.Eat(Item.ExtraParams[0]);
                break;

            case ItemAction.Placeable :
                Player.PlaceObject(Item);
                break;
        }
    }

    public void Harvest(int Resource, int Power, int Strength, string AnimName) {
        switch (Resource) {
            case 0 :
                Player.HarvestResource(Power, Strength, HarvestTool.Axe, AnimName);
                break;
            
            case 1 :
                Player.HarvestResource(Power, Strength, HarvestTool.Pickaxe, AnimName);
                break;

            case 2 :
                Player.HarvestResource(Power, Strength, HarvestTool.Weapon, AnimName);
                break;
        }
    }  


    private Player Player;

    void Awake() {
        Player = FindObjectOfType<Player>();
    }
}
