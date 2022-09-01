using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public enum WeaponType { SWORD, FIREBALL };
    [SerializeField] private WeaponType weapon = WeaponType.FIREBALL;

    private Vector3 attackLocation;
    private float attackDir;
    private bool canAttack = true;

    [SerializeField] private Camera cam;

    [SerializeField] private LayerMask lm_enemies;

    // Sword Variables
    [Header("Sword Variables")]
    [SerializeField] private Transform pfSwordSlash;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 5f;

    // Fireball Variables
    [Header("Fireball Variables")]
    [SerializeField] private Transform pfFireball;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float castTime = 1f;

    private void Update() {
        if (Input.GetMouseButton(0)) {
            Attack();
        }
    }
    
    private void Attack() {
        if (canAttack == false)
            return;

        switch (weapon) {
            case WeaponType.SWORD:
                SwordAttack();
                break;
            case WeaponType.FIREBALL:
                FireballAttack();
                break;
            default:
                Debug.Log("ERROR: No weapon type - how did we get here?");
                break;
        }
    }

    private void SwordAttack() {
        if (cam.ScreenToWorldPoint(Input.mousePosition).x > this.gameObject.transform.position.x)
            attackDir = 1f;
        if (cam.ScreenToWorldPoint(Input.mousePosition).x < this.gameObject.transform.position.x)
            attackDir = -1f;

        attackLocation = new Vector3(this.gameObject.transform.position.x - attackDir * 0.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        Transform swordSlash = Instantiate(pfSwordSlash, attackLocation + new Vector3(-attackDir, 0, 0), Quaternion.identity, null); //this.gameObject.transform
        swordSlash.localScale = new Vector3(1 * -attackDir, 1, 1);

        Collider2D[] hits = Physics2D.OverlapAreaAll(attackLocation, new Vector2(attackLocation.x + attackRange * attackDir, 0), lm_enemies);
        foreach (Collider2D enemy in hits) {
            if (enemy.GetComponent<BossController>() != null)
            {
                enemy.GetComponent<BossController>().Damage();
                Debug.Log("Hit boss");
                
            }
             else if(enemy.GetComponent<EnemyAiController>() != null)
            {
                enemy.GetComponent<EnemyAiController>().Damage(attackDamage, 5, attackDir);
                Debug.Log("SHiiit");
                //EnemyController merged with other AI behaviour

            }
            //enemy.GetComponent<EnemyController>().Damage(attackDamage, 5, attackDir);//Dans code works on this
        }

        canAttack = false;
        StartCoroutine(AttackReset(attackDuration));
    }

    private void FireballAttack() {
        if (cam.ScreenToWorldPoint(Input.mousePosition).x > this.gameObject.transform.position.x)
            attackDir = 1f;
        if (cam.ScreenToWorldPoint(Input.mousePosition).x < this.gameObject.transform.position.x)
            attackDir = -1f;

        attackLocation = new Vector3(this.gameObject.transform.position.x + attackDir * 0.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        Transform fireball = Instantiate(pfFireball, attackLocation + new Vector3(attackDir, 0, 0), Quaternion.identity, this.gameObject.transform);
        fireball.GetComponent<FireballController>().Create(castTime, projectileSpeed, attackDir);

        canAttack = false;
        StartCoroutine(AttackReset(castTime));
    }

    private IEnumerator AttackReset(float resetDuration) {
        yield return new WaitForSeconds(resetDuration);
        canAttack = true;
    }
}
