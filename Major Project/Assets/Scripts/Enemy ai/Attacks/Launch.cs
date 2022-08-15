using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Launch")]
public class Launch : Attack
{
    public override void DoAttack(EnemyAiController states, Vector2 distance, GameObject parent)
    {
        Debug.Log("Launcher no launch yet");
    }
}
