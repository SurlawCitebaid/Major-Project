using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundSpike : MonoBehaviour {
    [SerializeField] private float delay;
    [SerializeField] private Transform pfSpike;

    private void Start() {
        Invoke("SpawnSpike", delay);
    }

    private void SpawnSpike() {
        Instantiate(pfSpike, this.gameObject.transform.position, Quaternion.identity, null);
        Destroy(this.gameObject);
    }
}
