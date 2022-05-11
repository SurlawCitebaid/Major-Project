using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(enemy.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
