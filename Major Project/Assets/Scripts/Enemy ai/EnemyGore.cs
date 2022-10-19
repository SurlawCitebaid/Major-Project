using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGore : MonoBehaviour
{

    public List<Sprite> goreChunks;
    Sprite goreChunk;
    public float thrust = 2000f;
    // Start is called before the first frame update
    void Start()
    {
        //Selects random gore chunks
        gameObject.GetComponent<SpriteRenderer>().sprite = goreChunks[(int)Random.Range(0, goreChunks.Count)];
        explode();
    }

    public void explode()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * thrust, ForceMode2D.Impulse);
        Destroy(gameObject, 3);
    }
}
