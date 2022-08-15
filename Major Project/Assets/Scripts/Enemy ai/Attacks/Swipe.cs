using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Swipe")]
public class Swipe : Attack
{
    public override void DoAttack(EnemyAiController states, Vector2 distance, GameObject parent)
    {
        Debug.Log("Swiper no swipe yet");
    }
}
