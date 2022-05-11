using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public new string name;
    public bool boss;
    public enum TYPE {GROUND, AERIAL, STATIONARY};//DOESNT DO ANYTHING
    public Sprite sprite;
    public int health;
    public int damage;
    public float moveSpeed;//base move speed
    public float slowSpeed = 0.5f; //speed multiplier when slowed
    public float stunSpeed = 0.0f; //speed multiplier when stunned
    public float range; //distance from the player required to initiate attack
    public float jumpHeight; //how high the entity can jump, if cannot jump set to 0

}