using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {
    [SerializeField] private float knockbackForce = 20f;
    [SerializeField] private float invulnerabilityDelay = 1f;
    [SerializeField] private Color32 defaultColour = new Color32();

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool canHurt = true;

    private void Awake() {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        sr.color = defaultColour;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!canHurt)
            return;

        if (collision.tag == "Hazard")
            DamagePlayer(collision.gameObject.transform.position);
    }

    private IEnumerator DamageReset() {
        yield return new WaitForSeconds(invulnerabilityDelay);
        canHurt = true;
    }

    private IEnumerator HitFlash() {
        sr.color = Color.white;
        yield return new WaitForSeconds(invulnerabilityDelay);
        sr.color = defaultColour;
    }

    private void DamagePlayer(Vector3 collisionPos) {
        canHurt = false;
        this.gameObject.GetComponent<HealthManager>().Damage();
        Vector3 knockbackDir = new Vector3((this.gameObject.transform.position.x - collisionPos.x), 0.3f).normalized;
        Knockback(knockbackForce, knockbackDir);
        StartCoroutine(DamageReset());
        StartCoroutine(HitFlash());
    }

    private void Knockback(float knockbackForce, Vector3 knockbackDirection) {
        rb.velocity = knockbackDirection * knockbackForce;
    }
}
