using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : Attack
{
    public override void DoAttack(EnemyAiController states, Vector2 distance, GameObject parent)
    {
        Debug.Log("Ranger threw");

        Vector2 direction = new Vector2(distance.normalized.x,0);
        Vector2 attackLocation = (Vector2)parent.transform.position + new Vector2(states.enemy.attack.maxRange - 0.5f, 0);
        Transform rangeA = Instantiate(animation, attackLocation, Quaternion.identity, parent.transform); //this.gameObject.transform
        
        //swipeA.localScale = new Vector3(1 * direction.x, 1, 1);

        Collider2D[] hitbox = Physics2D.OverlapBoxAll((Vector2)parent.transform.position + direction, rangeA.localScale,0,LayerMask.GetMask("Player"));
        foreach (var item in hitbox)
        {
            if(item.tag == "Player"){
                Debug.Log("Swiper HIT!");
                var player = item.gameObject.GetComponent<PlayerController>();
                player.damage(5);
                break;
            }
        }
    }
}