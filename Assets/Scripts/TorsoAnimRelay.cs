using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoAnimRelay : MonoBehaviour
{
    private Player Player;

    void Start() {
        Player = transform.parent.GetComponent<Player>();
    }

    public void ReturnControlToRun() {
        Debug.Log("Return");
        Player.ResetAnimDirection();
    }

    public void Break() {
        Debug.Log("Harvest");
        Player.Harvest();
    }
}
