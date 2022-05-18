using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = this.gameObject.GetComponent<Animator>();
    }

    private void Update() {
        if (animator == null)
            return;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Kill"))
            Destroy(this.gameObject);
    }
}
