using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Charge")]
public class Charge : Attack
{
    
    public override void DoAttack(EnemyAiController states, Vector2 distance){
        float chargeSpeed = 20f; //speed of the charging attack
        states.changeVelocity(new Vector2(distance.normalized.x * chargeSpeed, states.getYVelocity()));

        //animate
        //attackLocation = new Vector3(this.gameObject.transform.position.x - attackDir * 0.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        //Vector2 location
        //Transform swordSlash = Instantiate(animation, attackLocation + new Vector3(-attackDir, 0, 0), Quaternion.identity, null); //this.gameObject.transform
        //swordSlash.localScale = new Vector3(1 * -attackDir, 1, 1);
    }
}
