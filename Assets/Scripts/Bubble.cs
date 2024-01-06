using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public List<Sprite> BubbleStates;
    private SpriteRenderer Renderer;
    private int Count = -1;

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(BubbleAnim());
    }

    IEnumerator BubbleAnim() {
        yield return new WaitForSeconds(Random.Range(0, 10));

        while (true) {
            Count++;

            if (Count == BubbleStates.Count)
                Count = 0;

            Renderer.sprite = BubbleStates[Count];

            if (Count == 0)
                yield return new WaitForSeconds(Random.Range(3, 10));
            else
                yield return new WaitForSeconds(Random.Range(0.4f, 1));
        }
    }
}
