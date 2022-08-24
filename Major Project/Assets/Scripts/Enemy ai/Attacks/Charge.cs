using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Charge")]
public class Charge : Attack
{
    
    public override void DoAttack(EnemyAiController states, Vector2 distance, GameObject parent){
        float chargeSpeed = 20f; //speed of the charging attack
        states.changeVelocity(new Vector2(distance.normalized.x * chargeSpeed, states.getYVelocity()));

        //animate
        Vector2 attackLocation = parent.transform.position;
        Vector2 direction = new Vector2(distance.normalized.x,0);
        Transform chargeA = Instantiate(animation, attackLocation, Quaternion.identity, parent.transform); //this.gameObject.transform
        chargeA.localScale = new Vector3(1 * direction.x, 1, 1);

        Collider2D[] hitbox = Physics2D.OverlapBoxAll((Vector2)parent.transform.position + direction, chargeA.localScale,0,LayerMask.GetMask("Player"));
        
        foreach (var item in hitbox)
        {
            if(item.tag == "Player"){
                Debug.Log("Charger HIT!");
                break;
            }
        }
        
        
    }
}
