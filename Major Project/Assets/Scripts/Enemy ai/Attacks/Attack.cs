using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Default")]
/*
    Children of this contain the logic for the attacks
*/
public abstract class Attack : MonoBehaviour
{
   public abstract void DoAttack(EnemyAiController states, Vector2 distance, GameObject parent);

   public Transform animation;

}
