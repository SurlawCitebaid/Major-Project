using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private bool canAttack = true, isAttacking = false, isCombo = false, genArrow = false;
    private GameObject arrow, fireBallArrow;
    private Transform firePoint;
    private int comboCount = 0;

    public enum WeaponType { SWORD, FIST, FIREBALL};
    [Header("General")][Space]
    public WeaponType weapon = WeaponType.FIREBALL;

    public float maxCharge,damage, delayTime, attackSize = 0;
    // Sword Variables
    [Header("Sword Variables")]
    [SerializeField] private GameObject Arrow, Sword, ChargedSword;
    private float chargeTime = 0, comboResetTime = 0, maxChargeTime = 1.5f;

    private SpriteRenderer arrowColor, fireBallArrowColor;
    // Fireball Variables
    [Header("Fireball Variables")]
    [SerializeField] private Transform pfFireball, pfBigFireball;

    [Header("Punch Variables")]
    [SerializeField] private GameObject Punch;

    [Header("Icon")][Space]
    public GameObject m_Fireball;
    public GameObject m_Ranged;
    public GameObject m_Fist;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        fireBallArrow = firePoint.transform.Find("Arrow").gameObject;
        fireBallArrowColor = fireBallArrow.GetComponent<SpriteRenderer>();
    }
    private void Update() {

        if (Time.timeScale == 1)
        {
            delayTime = PlayerController.Instance.delayTime;
            attackSize = PlayerController.Instance.attackSize;
            damage = PlayerController.Instance.baseDamage;
            maxCharge = maxChargeTime - delayTime;

            if (!genArrow && weapon == WeaponType.SWORD)                                        //check if weapon is sword and arrow is not spawned
            {
                arrow = Instantiate(Arrow, transform);
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

                arrow.transform.SetPositionAndRotation(transform.position + direction, Quaternion.AngleAxis(angle - 90, Vector3.forward));

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
                if (chargeTime >= maxCharge) { fireBallArrowColor.color = Color.red; }
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
            chargeTime += Time.deltaTime;
        }
        if(Input.GetMouseButtonUp(0) && chargeTime >= maxCharge)
        {
            chargeTime = 0;
            Transform bigBall = Instantiate(pfBigFireball, firePoint.position, firePoint.rotation);
            bigBall.GetComponent<FireballController>().damage = damage*4;
            bigBall.localScale = new Vector2(bigBall.transform.localScale.x + attackSize, bigBall.transform.localScale.y + attackSize);
            canAttack = false;
            fireBallArrowColor.color = Color.white;
            FindObjectOfType<AudioManager>().Play("PlayerFireballBig");
            StartCoroutine(attackDelay(1f - delayTime));

        } else if (Input.GetMouseButtonUp(0) && chargeTime < maxCharge)
        {
            chargeTime = 0;
            Transform smallBall = Instantiate(pfFireball, firePoint.position, firePoint.rotation);

            smallBall.GetComponent<FireballController>().damage = damage;
            smallBall.localScale = new Vector2(smallBall.transform.localScale.x + attackSize, smallBall.transform.localScale.y + attackSize);
            canAttack = false;
            FindObjectOfType<AudioManager>().Play("PlayerFireballSmall");
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
            FindObjectOfType<AudioManager>().Play("SwordAttack");
            GameObject sword = Instantiate(ChargedSword, this.transform);
            sword.transform.localScale = new Vector2((sword.transform.localScale.x + attackSize)*2, (sword.transform.localScale.y + attackSize)*2);
            StartCoroutine(attackDelay(.6f - delayTime));
        }
        else if (Input.GetMouseButtonUp(0) && chargeTime < maxCharge)
        {
            chargeTime = 0;
            canAttack = false;
            FindObjectOfType<AudioManager>().Play("SwordAttack");
            GameObject sword = Instantiate(Sword, arrow.transform.position, arrow.transform.rotation * Quaternion.Euler(0f, 0f, 270f));
            sword.transform.localScale = new Vector2(sword.transform.localScale.x + attackSize, sword.transform.localScale.y + attackSize);
            sword.GetComponent<SwordDmg>().damage = damage/2;
            StartCoroutine(attackDelay(.6f - delayTime));
        }
    }
    private void fistAttack()
    {
        
        if (Input.GetMouseButtonDown(0) && comboCount == 0 && canAttack)
        {
            isAttacking = true;
            isCombo = true;
            canAttack = false;
            GameObject punch = Instantiate(Punch, firePoint.transform.position, this.transform.rotation);
            punch.GetComponent<PunchController>().time = 0.2f;
            punch.GetComponent<PunchController>().damage = damage;
            StartCoroutine(attackDelay(.2f)); // 0.2 sec cd between punches
        }
        if (Input.GetMouseButtonDown(0) && comboCount == 1 && canAttack)
        {
            isAttacking = true;
            canAttack = false;
            GameObject punch = Instantiate(Punch, firePoint.transform.position, this.transform.rotation);
            punch.GetComponent<PunchController>().time = 0.2f;
            punch.GetComponent<PunchController>().damage = damage;
            StartCoroutine(attackDelay(.2f)); 
        }
        if (Input.GetMouseButtonDown(0) && comboCount == 2 && canAttack)
        {
            isAttacking = true;
            canAttack = false;
            GameObject punch = Instantiate(Punch, firePoint.transform.position, this.transform.rotation);
            punch.GetComponent<PunchController>().time = 1.0f;
            punch.GetComponent<PunchController>().damage = damage * 2;
            StartCoroutine(attackDelay(1f)); 
        }
    }
    public bool GetAttacking()
    {
        return isAttacking;
    }
    public int GetComboCount()
    {
        return comboCount;
    }
    public void cycleWeapon()
    {
        switch(weapon)
        {
            case WeaponType.FIST:
                weapon = WeaponType.SWORD;
                StartCoroutine(DisplayWeaponIcon(m_Ranged));
                break;
            case WeaponType.SWORD:
                weapon = WeaponType.FIREBALL;
                StartCoroutine(DisplayWeaponIcon(m_Fireball));
                break;
            case WeaponType.FIREBALL:
                weapon = WeaponType.FIST;
                StartCoroutine(DisplayWeaponIcon(m_Fist));
                break;
            default:
                break;
        }
    }
    IEnumerator DisplayWeaponIcon(GameObject Icon)
    {
        Vector3 position = new();
        position.Set(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
        GameObject icon = Instantiate(Icon, position, this.transform.rotation);
        icon.transform.SetParent(this.gameObject.transform);
        yield return new WaitForSeconds(1);
        Destroy(icon);
    }
}
