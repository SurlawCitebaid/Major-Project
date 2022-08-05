using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {
    public enum State { CASTING, FIRING };
    public State state { get; private set; }

    private ParticleSystem ps;

    private float castTime;
    private float projectileSpeed;
    private float attackDir;

    private void Start() {
        ps = this.gameObject.GetComponent<ParticleSystem>();
        state = State.CASTING;
    }

    public void Create(float _castTime, float _projectileSpeed, float _attackDir) {
        castTime = _castTime;
        projectileSpeed = _projectileSpeed;
        attackDir = _attackDir;
        StartCoroutine(Fire(castTime));
    }

    private IEnumerator Fire(float delay) {
        yield return new WaitForSeconds(delay);
        state = State.FIRING;
        Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = Vector2.right * attackDir * projectileSpeed;
        this.gameObject.transform.parent = null;
        ps.subEmitters.AddSubEmitter(this.gameObject.GetComponentInChildren<ParticleSystem>(), ParticleSystemSubEmitterType.Birth, ParticleSystemSubEmitterProperties.InheritNothing);
    }
}
