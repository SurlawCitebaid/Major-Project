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
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, .5f);
        foreach (Collider2D coll in array)
        {
            PlayerController ass = coll.gameObject.GetComponent<PlayerController>();
            
            if (ass != null)                // checks player
            {
                Destroy(gameObject);
                ass.damage(1);
            }
            if(coll.gameObject.CompareTag("Wall"))           //checks wall
            {
                Destroy(gameObject);

            }
        }
        float moveSpeed = 20f;
        transform.Translate(shootDir * moveSpeed * Time.deltaTime);
    }
}
