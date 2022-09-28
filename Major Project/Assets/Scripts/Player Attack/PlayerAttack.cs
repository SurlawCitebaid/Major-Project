using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Transform firePoint;
    private GameObject hat;
    public enum WeaponType { FIST, SWORD, FIREBALL};
    [Header("General")][Space]
    public movement_Mario m_movementController;
    [SerializeField] private WeaponType weapon = WeaponType.FIREBALL;
    private Vector3 attackLocation;
    private float attackDir = 1f;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isCombo = false;
    private int comboCount = 0;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask lm_enemies;
    [Space][Space]

    // Sword Variables
    [Header("Sword Variables")]
    [SerializeField] private Transform pfSwordSlash;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 5f;
    [Space]
    [Space]
    private float chargeTime = 0;
    private float comboResetTime = 0;
    private float maxCharge = 2f;
    // Fireball Variables
    [Header("Fireball Variables")]
    [SerializeField] private Transform pfFireball, pfBigFireball;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        hat = transform.Find("Hat").gameObject;
    }
    private void Update() {
        if (canAttack)                                   // adds delay
        {
            Attack();
        }

        // if no attack input in 2 seconds from 1st attack. reset combo
        if (isCombo)
        {
            if(Input.GetMouseButton(0))
            {
                comboResetTime = 0;
                comboCount ++;
            }
            comboResetTime += Time.deltaTime;
            if(comboResetTime == 2.0f)
            {
                comboResetTime = 0;
                comboCount = 0;
                isCombo = false;
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
            case WeaponType.FIST:
                fistAttack();
                break;
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
    private void FireballAttack()
    {
        
        if (Input.GetMouseButton(0) && chargeTime < maxCharge)
        {
            isAttacking = true;
            chargeTime += Time.deltaTime;
            hat.GetComponent<SpriteRenderer>().enabled = true;

            if (chargeTime >= maxCharge) { hat.GetComponent<Animator>().enabled = true; }
        }
        if(Input.GetMouseButtonUp(0) && chargeTime >= maxCharge)
        {
            chargeTime = 0;
            Instantiate(pfBigFireball, firePoint.position, firePoint.rotation);
            canAttack = false;
            hat.GetComponent<Animator>().enabled = false;
            hat.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(attackDelay(.5f));

        } else if (Input.GetMouseButtonUp(0) && chargeTime < maxCharge)
        {
            chargeTime = 0;
            Instantiate(pfFireball, firePoint.position, firePoint.rotation);
            canAttack = false;
            hat.GetComponent<Animator>().enabled = false;
            hat.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(attackDelay(.5f));
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
                enemy.GetComponent<BossController>().Damage(1);
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

    private void fistAttack()
    {
        
        if (Input.GetMouseButton(0) && comboCount == 0)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            StartCoroutine(attackDelay(.2f)); // 0.2 sec cd between punches
        }
        if (Input.GetMouseButton(0) && comboCount == 1)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            StartCoroutine(attackDelay(.2f)); 
        }
        if (Input.GetMouseButton(0) && comboCount == 2)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            comboCount = 0;                     // reset combo to 0
            StartCoroutine(attackDelay(1.5f));  // 1.5 sec cd at the end of combo
        }
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
