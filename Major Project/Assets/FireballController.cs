using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {
    public enum State { CASTING, FIRING };
    public State state { get; private set; }

    private Rigidbody2D rb;
    private ParticleSystem ps;
    [SerializeField] private TrailRenderer trail;
    private Vector3 offset;

    private float castTime;
    private float projectileSpeed;
    private float attackDir;

    private void Start() {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        ps = this.gameObject.GetComponent<ParticleSystem>();
        state = State.CASTING;
        offset = this.gameObject.transform.position;
    }

    private void Update() {
        this.gameObject.transform.position = this.gameObject.transform.parent.position + offset;
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
        rb.velocity = Vector2.right * attackDir * projectileSpeed;
        this.gameObject.transform.parent = null;
        UpdateParticleSystem();
    }

    private void UpdateParticleSystem() {
        
    }
}
