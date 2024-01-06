using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public float Lifetime;

    private float time;

    void Awake() {
        transform.parent = null;
    }

    void FixedUpdate() {
        time += Time.fixedDeltaTime;
        if (time >= Lifetime) {
            Destroy(gameObject);
        }
    }
}
