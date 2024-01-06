using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField]
    List<Sprite> Sprites;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[Random.Range(0, Sprites.Count - 1)];
    }
}
