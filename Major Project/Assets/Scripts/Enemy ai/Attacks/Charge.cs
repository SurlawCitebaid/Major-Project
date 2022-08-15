using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Charge")]
public class Charge : Attack
{
    
    public override void DoAttack(EnemyAiController states, Vector2 distance, Vector2 position){
        float chargeSpeed = 20f; //speed of the charging attack
        states.changeVelocity(new Vector2(distance.normalized.x * chargeSpeed, states.getYVelocity()));

        //animate
        Vector2 attackLocation = position;
        Vector2 direction = new Vector2(distance.normalized.x,0);
        Transform chargeA = Instantiate(animation, attackLocation + new Vector2(direction.x, 0), Quaternion.identity, null); //this.gameObject.transform
        chargeA.localScale = new Vector3(1 * direction.x, 1, 1);
    }
}
