using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Sprite;
    private GameObject player;
    private Vector3 shootDir;
    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 vectorToTarget = player.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Sprite.transform.rotation = q;
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
