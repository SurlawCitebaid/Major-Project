using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetRotation();
        StartCoroutine(Remove());
    }

    void SetRotation(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 spawnPoint = transform.position + new Vector3(0.5f, 0, 0);
        Vector3 vectorToTarget = player.transform.position - spawnPoint;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = q;
    }

    IEnumerator Remove(){
        yield return new WaitForSeconds(2);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 2f);
        }
    }

}
