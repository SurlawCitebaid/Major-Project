using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
public class PlayerSO : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float damageResistance;
    public float speed;
    public float attackCooldown;
    public float grappleCoolDown;
    public float jumpForce;
    public float jumpAmount;
    public float dashTime;
    public float dashCooldown;
}
