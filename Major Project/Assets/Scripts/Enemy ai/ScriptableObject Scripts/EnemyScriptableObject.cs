using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public new string name;
    public string description; //general description of what the enemy does
    public bool boss; //Useless
    public Sprite sprite;
    public int health;
    public EnemyAttackScriptableObject attack;
    public GameObject enemyDeathParticles;
    //public int damage;
    //[Header("Attack Ranges")]
    //public float range; //distance from the player required to initiate attack
    //public float maxRange; //distance enemy will attempt to move to the player if trying to attack
    //public float minRange; //enemy will attempt to move away from the player, too close
    [Header("Movement")]
    public float moveSpeed; //base move speed
    public float slowSpeed = 0.5f; //speed multiplier when slowed
    public float stunSpeed = 0.0f; //speed multiplier when stunned
    public float jumpHeight; //how high the entity can jump, if cannot jump set to 0

    //Gore Effects
    [Header("Gore")]
    public Color goreColor;
    public GameObject goreChunks;
}
