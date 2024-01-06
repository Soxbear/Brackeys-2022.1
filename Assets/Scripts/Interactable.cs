using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractType InteractType;
    public Item Item;
    public int Count;
    public Sprite ToBeCollectedSprite;
    public Sprite CollectedSprite;
    public int RefreshTime;
    public string Menu;
    public LayerMask PlayerMask;
    private bool Used;
    private float time;
    public bool Finish;
    public bool DestroyOnPickup;

    public void FixedUpdate() {
        
        if (!Used) {

        if (Physics2D.OverlapCircle(transform.position, 1f, PlayerMask)) {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
            transform.GetChild(0).gameObject.SetActive(false);

        }
        else {
            if (RefreshTime != -1) {
                time += Time.fixedDeltaTime;
                if (time >= RefreshTime) {
                    GetComponent<SpriteRenderer>().sprite = ToBeCollectedSprite;
                    transform.GetChild(1).gameObject.layer = 8;
                    Used = false;
                    time = 0;
                }
            }
        }
        
    }

    public void Collected() {
        if (Finish) {
            if (FindObjectOfType<Player>().HasItem()) {
                Debug.Log("Yes");
            FindObjectOfType<TeleHandler>().Finish();
            return;
            }
            return;
        }
        GetComponent<SpriteRenderer>().sprite = CollectedSprite;
        transform.GetChild(1).gameObject.layer = 9;
        Used = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}

public enum InteractType {
    Collect,
    UI,

}