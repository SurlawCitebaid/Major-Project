using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpikeAttack : MonoBehaviour {
    [SerializeField] private float delay;
    [SerializeField] private Transform pfSpawnObject;
    
    private Transform playerPos;

    private void Awake() {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start() {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject() {
        yield return new WaitForSeconds(delay);
        Vector3 spawnPos = playerPos.position;
        spawnPos.y = this.gameObject.transform.position.y;
        Instantiate(pfSpawnObject, spawnPos, Quaternion.identity, null);
        StartCoroutine(SpawnObject());
    }
}
