using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour {
    [SerializeField] private Vector3 float_value;
    [SerializeField] private float float_rate;

    private Vector3 default_pos;

    private void Awake() {
        default_pos = this.gameObject.transform.position;
    }

    private void Update() {
        this.gameObject.transform.position = default_pos + float_value * Mathf.Sin(Time.time * float_rate);
    }
}
