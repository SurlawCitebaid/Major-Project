using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private bool canAttack = true, isAttacking = false, isCombo = false, genArrow = false;
    private GameObject arrow, fireBallArrow;
    private Transform firePoint;
    private int comboCount = 0;

    public enum WeaponType { FIST, SWORD, FIREBALL};
    [Header("General")][Space]
    [SerializeField] private WeaponType weapon = WeaponType.FIREBALL;


    public float damage, delayTime, attackSize = 0;



    // Sword Variables
    [Header("Sword Variables")]
    [SerializeField] private GameObject Arrow, Sword, ChargedSword;
    private float chargeTime = 0, comboResetTime = 0, maxCharge = 1.5f;

    private SpriteRenderer arrowColor, fireBallArrowColor;
    // Fireball Variables
    [Header("Fireball Variables")]
    [SerializeField] private Transform pfFireball, pfBigFireball;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        fireBallArrow = firePoint.transform.Find("Arrow").gameObject;
        fireBallArrowColor = fireBallArrow.GetComponent<SpriteRenderer>();
    }
    private void Update() {
        foreach (ItemValues item in Inventory.instance.inventory)
        {
            if (item.GetName() == "Pill" || item.GetName() == "Magnifying Glass")
            {
                if (item.GetName() == "Pill")
                {
                    delayTime = item.GetAmount() * .2f;
                }
                if (item.GetName() == "Magnifying Glass")
                {
                    attackSize = item.GetAmount() * .2f;
                }
            }
        }

        if (Time.timeScale == 1)
        {
            damage = transform.GetComponent<PlayerController>().baseDamage;
            if (!genArrow && weapon == WeaponType.SWORD)                                        //check if weapon is sword and arrow is not spawned
            {
                arrow = Instantiate(Arrow, firePoint.transform.position, firePoint.transform.rotation);
                arrowColor = arrow.transform.Find("Arrow").GetComponent<SpriteRenderer>();

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

                if (chargeTime >= maxCharge){arrowColor.color = Color.red;}
                else{arrowColor.color = Color.white;}                                           //checks if charged attack is ready or not

            } else if (weapon != WeaponType.SWORD && genArrow)                                  //check if weapon changed and arrow spawned is true
            {
                Destroy(arrow);
                genArrow = false;
            }
            if(weapon != WeaponType.FIREBALL)
            {
                fireBallArrow.SetActive(false);
            } else
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    firePoint.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);                     //face up
                    firePoint.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else if(!Input.GetKey(KeyCode.W))
                {
                    
                    if (gameObject.transform.rotation.y > 0)
                    {    
                        firePoint.position = new Vector2(gameObject.transform.position.x + .5f, gameObject.transform.position.y);               //face right
                        firePoint.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    if (gameObject.transform.rotation.y < 0)
                    {
                        firePoint.position = new Vector2(gameObject.transform.position.x - .5f, gameObject.transform.position.y);               //face left
                        firePoint.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                }
                
            }
            if (isCombo)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    comboResetTime = 0;
                }
                comboResetTime += Time.deltaTime;
                if (comboResetTime >= 2.0f)
                {
                    comboResetTime = 0;
                    comboCount = 0;
                    isCombo = false;
                }
            }
            if (canAttack)                                   // adds delay
            {
                Attack();
            }
        }

        // if no attack input in 2 seconds from last attack. reset combo
       
    }
    IEnumerator attackDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (weapon == WeaponType.FIST && comboCount < 2)
        {
            comboCount++;
        } else if (weapon == WeaponType.FIST && comboCount == 2)
        {
            comboCount = 0;
        }
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
                fireBallArrow.SetActive(true);
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

            if (chargeTime >= maxCharge) { fireBallArrowColor.color = Color.red; }
        }
        if(Input.GetMouseButtonUp(0) && chargeTime >= maxCharge)
        {
            chargeTime = 0;
            Transform bigBall = Instantiate(pfBigFireball, firePoint.position, firePoint.rotation);
            bigBall.GetComponent<FireballController>().damage = damage*2;
            bigBall.localScale = new Vector2(bigBall.transform.localScale.x + attackSize, bigBall.transform.localScale.y + attackSize);
            canAttack = false;
            fireBallArrowColor.color = Color.white;
            StartCoroutine(attackDelay(1f - delayTime));

        } else if (Input.GetMouseButtonUp(0) && chargeTime < maxCharge)
        {
            chargeTime = 0;
            Transform smallBall = Instantiate(pfFireball, firePoint.position, firePoint.rotation);

            smallBall.GetComponent<FireballController>().damage = damage;
            smallBall.localScale = new Vector2(smallBall.transform.localScale.x + attackSize, smallBall.transform.localScale.y + attackSize);
            canAttack = false;
            StartCoroutine(attackDelay(1f - delayTime));
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
            GameObject sword = Instantiate(ChargedSword, this.transform);
            sword.transform.localScale = new Vector2(sword.transform.localScale.x + attackSize, sword.transform.localScale.y + attackSize);
            StartCoroutine(attackDelay(1f - delayTime));
        }
        else if (Input.GetMouseButtonUp(0) && chargeTime < maxCharge)
        {
            chargeTime = 0;
            canAttack = false;
            GameObject sword = Instantiate(Sword, arrow.transform.position, arrow.transform.rotation * Quaternion.Euler(0f, 0f, 270f));
            sword.transform.localScale = new Vector2(sword.transform.localScale.x + attackSize, sword.transform.localScale.y + attackSize);
            sword.GetComponent<SwordDmg>().damage = damage;
            StartCoroutine(attackDelay(1f - delayTime));
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
            canAttack = false;
            StartCoroutine(attackDelay(.2f)); // 0.2 sec cd between punches
        }
        if (Input.GetMouseButtonDown(0) && comboCount == 1 && canAttack)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            canAttack = false;
            StartCoroutine(attackDelay(.2f)); 
        }
        if (Input.GetMouseButtonDown(0) && comboCount == 2 && canAttack)
        {
            // ------------------------------- //
            //  need animator control here ()  //
            // ------------------------------- //
            isAttacking = true;
            canAttack = false;
            StartCoroutine(attackDelay(1.0f));  // 1.5 sec cd at the end of combo
        }
    }

    public bool getAttacking()
    {
        return isAttacking;
    }
    public string getWeaponType()
    {
        string wep = "";
        if(weapon == WeaponType.FIST)
        {
            wep = "FIST"; 
        }
        return wep;
    }
    public int getComboCount()
    {
        return comboCount;
    }
}
