using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepad : MonoBehaviour
{
    public Transform Destination;

    [HideInInspector]
    public bool IgnoreEnter;

    void OnTriggerEnter2D() {
        if (IgnoreEnter) {
            IgnoreEnter = false;
            return;
        }

        FindObjectOfType<TeleHandler>().Teleport(Destination.position, Destination.GetComponent<Telepad>());
    }
}
