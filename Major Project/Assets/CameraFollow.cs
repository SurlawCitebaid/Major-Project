using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float lerpSpeed;

    private Vector3 offset;

    private void Start() {
        offset = this.gameObject.transform.position - target.position;
    }

    private void Update() {
        if (target == null)
            return;

        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, target.position + offset, lerpSpeed);
    }
}
