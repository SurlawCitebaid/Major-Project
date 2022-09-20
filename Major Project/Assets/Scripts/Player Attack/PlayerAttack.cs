using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Transform firePoint;
    public enum WeaponType { SWORD, FIREBALL };
    [Header("General")][Space]
    public movement_Mario m_movementController;
    [SerializeField] private WeaponType weapon = WeaponType.FIREBALL;
    private Vector3 attackLocation;
    private float attackDir = 1f;
    private bool canAttack = true;
    private bool isAttacking = false;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask lm_enemies;
    [Space][Space]

    // Sword Variables
    [Header("Sword Variables")]
    [SerializeField] private Transform pfSwordSlash;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 5f;
    [Space][Space]

    // Fireball Variables
    [Header("Fireball Variables")]
    [SerializeField] private Transform pfFireball;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float castTime = 1f;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
    }
    private void Update() {
        if(canAttack)                                   // adds delay
        {
            if (Input.GetMouseButtonDown(0))
            {
                isAttacking = true;
                Attack();
                
                canAttack = false;
                StartCoroutine(attackDelay(.5f));
            }
        }
        
    }
    IEnumerator attackDelay(float delayTime)
    {
        
        yield return new WaitForSeconds(delayTime);
        isAttacking = false;
        canAttack = true;
    }
    private void Attack() {

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
        //if (cam.ScreenToWorldPoint(Input.mousePosition).x > this.gameObject.transform.position.x)
        //    attackDir = 1f;
        //if (cam.ScreenToWorldPoint(Input.mousePosition).x < this.gameObject.transform.position.x)
        //    attackDir = -1f;

        attackLocation = new Vector3(this.gameObject.transform.position.x - attackDir * 0.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

        Transform swordSlash = Instantiate(pfSwordSlash, firePoint.position, firePoint.rotation, gameObject.transform); //this.gameObject.transform
        //swordSlash.localScale = new Vector3(1 * attackDir, 1, 1);

        Collider2D[] hits = Physics2D.OverlapAreaAll(attackLocation, new Vector2(attackLocation.x + attackRange * attackDir, 0), lm_enemies);
        Collider2D[] hit = Physics2D.OverlapBoxAll(swordSlash.transform.position, swordSlash.transform.localScale, 0, lm_enemies);
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
    }

    private void FireballAttack() {            
        
        Instantiate(pfFireball, firePoint.position, firePoint.rotation );
    }

    public void setAttackDirection(string dir)         // set attack direction based on character facing
    {
        switch (dir)
        {
            case "left":
                attackDir = -1f;
                break;
            case "right":
                attackDir = 1f;
                break;
        }
    }

    public bool getAttacking()
    {
        return isAttacking;
    }
}
