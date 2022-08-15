using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Launch")]
public class Launch : Attack
{
    public override void DoAttack(EnemyAiController states, Vector2 distance, Vector2 position)
    {
        Debug.Log("Launcher no launch yet");
    }
}
