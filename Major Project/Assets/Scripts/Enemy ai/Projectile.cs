using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 shootDir;  
    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
    }
    private void Update()
    {
        float moveSpeed = 20f;
        transform.Translate(shootDir * moveSpeed * Time.deltaTime);
    }
}
