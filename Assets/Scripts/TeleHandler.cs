using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleHandler : MonoBehaviour
{
    private Animator Animator;

    private Transform Player;
    public Player PlayerComponent;

    private Vector2 Location;

    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.enabled = false;
        PlayerComponent = FindObjectOfType<Player>();
        Player = PlayerComponent.transform;
    }

    public void Teleport(Vector2 Destination, Telepad ToDisable = null) {
        Destination.y += 0.5f;
        Animator.enabled = true;
        Animator.Play("Telefade");
        Location = Destination;
        ToDisable.IgnoreEnter = true;
    }

    public void Finish() {
        Debug.Log("Pog");
        Animator.enabled = true;
        Animator.Play("Finish");
    }

    public void Die(Vector2 Respwan) {
        Animator.enabled = true;
        Animator.Play("Die");
        Location = Respwan;
    }

    public void DoTeleport() {
        Player.position = Location;
    }

    public void DisableMovement() {
        PlayerComponent.DisableMovement = true;
    }

    public void EnableMovement() {
        Debug.Log("Enable");
        PlayerComponent.DisableMovement = false;
    }

    public void DisableAnimator() {
        Animator.enabled = false;
    }

    public void Quit() {
        Application.Quit();
    }
}
