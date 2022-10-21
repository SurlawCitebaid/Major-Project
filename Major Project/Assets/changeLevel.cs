using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLevel : MonoBehaviour
{
    [SerializeField]
    string sceneName;

    // Update is called once per frame
    void Update()
    {
        //Boss is dead
        if(EnemySpawner.enemiesAlive == false)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(activatePortal());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Magnet"))
        {
            PlayerController.Instance.stages += 1;
            SceneManager.LoadScene(sceneName);
        }

            
    }

    IEnumerator activatePortal()
    {
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
