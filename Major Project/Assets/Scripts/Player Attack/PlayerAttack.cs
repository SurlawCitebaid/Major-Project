using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Transform firePoint;
    private GameObject hat, arrow;
    private bool genArrow = false;
    public enum WeaponType { FIST, SWORD, FIREBALL};
    [Header("General")][Space]
    public movement_Mario m_movementController;
    [SerializeField] private WeaponType weapon = WeaponType.FIREBALL;

    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isCombo = false;
    private int comboCount = 0;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask lm_enemies;
    [Space][Space]

    // Sword Variables
    [Header("Sword Variables")]
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject Sword;
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
        if (!genArrow && weapon == WeaponType.SWORD)                                        //check if weapon is sword and arrow is not spawned
        {
            arrow = Instantiate(Arrow, firePoint.transform.position, firePoint.transform.rotation);
            genArrow = true;
        } else if (weapon == WeaponType.SWORD && genArrow) {                                //check if weapon is sword and arrow spawned is true           
            Vector3 wPos = Input.mousePosition;
            wPos.z = transform.position.z - Camera.main.transform.position.z;
            wPos = Camera.main.ScreenToWorldPoint(wPos);
            Vector3 direction = wPos - transform.position;                                  //pos on circumference
            float radius = 1;
            direction = Vector3.Normalize(direction) * radius;

            Vector3 dir = transform.position - arrow.transform.position;                    //rotate the arrow
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            arrow.transform.position = transform.position + direction;
            arrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        } else if(weapon != WeaponType.SWORD && genArrow)                                   //check if weapon changed and arrow spawned is true
        {
            Destroy(arrow);
            genArrow = false;
        }

        if (canAttack)                                   // adds delay
        {
            Attack();
        }


        // if no attack input in 2 seconds from last attack. reset combo
        if (isCombo)
        {
            if(Input.GetMouseButtonDown(0))
            {
                comboResetTime = 0;
            }
            comboResetTime += Time.deltaTime;
            if(comboResetTime >= 2.0f)
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
        


        if (Input.GetMouseButton(0) && chargeTime < maxCharge)
        {
            chargeTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0) && chargeTime >= maxCharge)
        {
            chargeTime = 0;
            canAttack = false;

            StartCoroutine(attackDelay(.5f));

        }
        else if (Input.GetMouseButtonUp(0) && chargeTime < maxCharge)
        {
            chargeTime = 0;
            canAttack = false;
            Instantiate(Sword, arrow.transform.position, arrow.transform.rotation * Quaternion.Euler(0f, 0f, 270f));


            StartCoroutine(attackDelay(.5f));
        }
    }

    private void fistAttack()
    {
        
        if (Input.GetMouseButtonDown(0) && comboCount == 0 && canAttack)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            isCombo = true;
            comboCount++;
            canAttack = false;
            StartCoroutine(attackDelay(.2f)); // 0.2 sec cd between punches
        }
        if (Input.GetMouseButtonDown(0) && comboCount == 1 && canAttack)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            comboCount++;
            canAttack = false;
            StartCoroutine(attackDelay(.2f)); 
        }
        if (Input.GetMouseButtonDown(0) && comboCount == 2 && canAttack)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            comboCount = 0;
            canAttack = false;
            StartCoroutine(attackDelay(1.0f));  // 1.5 sec cd at the end of combo
        }
    }

    public bool getAttacking()
    {
        return isAttacking;
    }
}
