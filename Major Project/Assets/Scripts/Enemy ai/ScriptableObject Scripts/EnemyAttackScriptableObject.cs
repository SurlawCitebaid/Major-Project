using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObjects/Enemy Attack")]
public class EnemyAttackScriptableObject : ScriptableObject
{
    public int damage;
    public float range; //distance from the player required to initiate attack
    public float maxRange; //distance enemy will attempt to move to the player if trying to attack
    public float minRange; //enemy will attempt to move away from the player, too close
    public float cooldownTime; //time till attack will be performed again
    public float immunityTime; //time immune during attack
    //animation for attack in future
    
}
