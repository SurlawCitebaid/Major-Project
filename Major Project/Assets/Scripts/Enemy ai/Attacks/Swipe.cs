using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemy Attack/Swipe")]
public class Swipe : Attack
{
    public override void DoAttack(EnemyAiController states, Vector2 distance, GameObject parent)
    {

        Vector2 direction = new Vector2(distance.normalized.x,0);
        Vector2 attackLocation = (Vector2)parent.transform.position + direction;
        Transform swipeA = Instantiate(animation, attackLocation, Quaternion.identity, parent.transform); //this.gameObject.transform
        
        //swipeA.localScale = new Vector3(1 * direction.x, 1, 1);

        Collider2D[] hitbox = Physics2D.OverlapBoxAll((Vector2)parent.transform.position + direction, swipeA.localScale,0,LayerMask.GetMask("Player"));
        foreach (var item in hitbox)
        {
            if(item.tag == "Player"){
                Debug.Log("Swiper HIT!");
                var player = item.gameObject.GetComponent<PlayerController>();
                player.damage(states.enemy.attack.damage);
                break;
            }
        }
    }
}