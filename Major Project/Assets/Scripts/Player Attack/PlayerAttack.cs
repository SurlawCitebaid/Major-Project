using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Camera cam;

    [SerializeField] private LayerMask lm_enemies;
    [SerializeField] private Transform pfSwordSlash;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 5f;
    
    private Vector3 attackLocation;
    private float attackDir;
    private bool canAttack = true;

    private void Awake() {
        cam = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            Attack();
        }
    }
    
    private void Attack() {
        if (canAttack == false)
            return;

        if (cam.ScreenToWorldPoint(Input.mousePosition).x > this.gameObject.transform.position.x)
            attackDir = 1f;
        if (cam.ScreenToWorldPoint(Input.mousePosition).x < this.gameObject.transform.position.x)
            attackDir = -1f;

        attackLocation = new Vector3(this.gameObject.transform.position.x + attackDir * 0.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        Transform swordSlash = Instantiate(pfSwordSlash, attackLocation + new Vector3(attackDir, 0, 0), Quaternion.identity, this.gameObject.transform);
        swordSlash.localScale = new Vector3(1 * attackDir, 1, 1);

        Collider2D[] hits = Physics2D.OverlapAreaAll(attackLocation, new Vector2(attackLocation.x + attackRange * attackDir, 0), lm_enemies);
        foreach (Collider2D enemy in hits) {
            enemy.GetComponent<GroundEnemyController>().Damage(attackDamage, 5, attackDir); //EnemyController merged with other AI behaviour
            //enemy.GetComponent<EnemyController>().Damage(attackDamage, 5, attackDir);//Dans code works on this
        }

        canAttack = false;
        StartCoroutine(AttackReset(attackDuration));
    }

    private IEnumerator AttackReset(float resetDuration) {
        yield return new WaitForSeconds(resetDuration);
        canAttack = true;
    }
}
