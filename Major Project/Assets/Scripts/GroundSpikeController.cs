using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpikeController : MonoBehaviour {
    [SerializeField] private float delay;

    private Animator animator;
    private new PolygonCollider2D collider;

    private void Awake() {
        animator = this.gameObject.GetComponent<Animator>();
    }

    private void Start() {
        collider = this.gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
        Invoke("DestroySpike", delay);
    }

    private void LateUpdate() {
        Destroy(collider);
        collider = this.gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
    }

    private void DestroySpike() {
        animator.SetTrigger("DestroySpike");
    }
}
