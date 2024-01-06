using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Harvestable : MonoBehaviour
{
    public HarvestTool HarvestTool;
    public int MinPower;
    public int Durability;
    public bool DestroyInstant;
    public AudioClip WeakTool;
    public List<AudioClip> Harvesting;

    private AudioSource Audio;

    public List<Item> Drops;

    public GameObject ParticleEffect;

    private SpriteRenderer Renderer;

    public void Harvest(HarvestTool ToolType, int Power, int Strength) {
        if (ToolType != HarvestTool) return;

        StartCoroutine(HarvestFlash());

        if (Power < MinPower) {
            Audio.clip = WeakTool;
            Audio.pitch = Random.Range(0.9f, 1.3f);
            Audio.Play();
            return;
        }

        if (Harvesting.Count > 0) {
            Audio.clip = Harvesting[Random.Range(0, Harvesting.Count - 1)];
            Audio.pitch = Random.Range(0.9f, 1f);
            Audio.Play();
        }

        Durability -= Strength;

        if (Durability <= 0) {

            ItemUtility ItemUtility = FindObjectOfType<ItemUtility>();

            foreach (Item Drop in Drops) {
                ItemUtility.DropItem(transform.position, Drop);
            }
            if (ParticleEffect)
                Instantiate(ParticleEffect, transform);
            Destroy(Renderer);
            Destroy(GetComponent<Collider2D>());
            StopAllCoroutines();
            if (DestroyInstant)
                Destroy(gameObject);
            else
                Destroy(gameObject, 5);
        }
    }

    void Awake() {
        Audio = GetComponent<AudioSource>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator HarvestFlash() {
        Renderer.color = new Color(0.9f, 0.9f, 0.9f);
        yield return new WaitForSeconds(0.25f);
        Renderer.color = new Color(1, 1, 1);
        StopCoroutine(HarvestFlash());
    }
}

public enum HarvestTool {
    Axe,
    Pickaxe,
    Weapon
}