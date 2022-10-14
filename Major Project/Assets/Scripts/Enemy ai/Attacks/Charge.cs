using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Attack
{

    float chargeSpeed = 20f; //speed of the charging attack
    Vector2 direction;
    GameObject parent;
    public override void DoAttack(EnemyAiController states, Vector2 distance, GameObject parent){
        
        states.ChangeVelocity(new Vector2(distance.normalized.x * chargeSpeed, states.GetYVelocity()));

        //animate
        this.parent = parent;
        direction = new Vector2(distance.normalized.x,0);
        Vector2 attackLocation = parent.transform.position;
        Transform chargeA = Instantiate(animation, attackLocation, Quaternion.identity, parent.transform); //this.gameObject.transform
        //chargeA.localScale = new Vector3(-1, 1, 1);

        StartCoroutine(Hit(chargeA, states.enemy.attack.damage));

        
        
    }

    IEnumerator Hit(Transform anim, int damage){
        bool hit = false;
        Collider2D[] hitbox;
        for(float i = 0; i < 1f; i += Time.deltaTime){//check each frame of charge for one second, stops when finds player
            if(anim == null) break;

            hitbox = Physics2D.OverlapBoxAll((Vector2)parent.transform.position + direction, anim.localScale,0,LayerMask.GetMask("Player"));
            foreach (var item in hitbox)
                {
                if(item.tag == "Player"){
                    hit = true;
                    //make player take damage
                    var player = item.gameObject.GetComponent<PlayerController>();
                    player.damage(damage);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
            if (hit) break;
        }
    }
    
}
