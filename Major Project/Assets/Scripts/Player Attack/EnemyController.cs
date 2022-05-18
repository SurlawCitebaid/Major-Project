using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private Color32 defaultColour = new Color32();

    private void Awake() {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        sr.color = defaultColour;
    }
    
    public void Damage(float damageAmount, float knockbackForce, float knockbackDirection) {
        Debug.Log("Hit for " + damageAmount);
        Knockback(knockbackForce, knockbackDirection);
        StartCoroutine(HitFlash());
    }

    private void Knockback(float knockbackForce, float knockbackDirection) {
        rb.AddForce(Vector2.right * knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator HitFlash() {
        sr.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        sr.color = defaultColour;
    }
}
