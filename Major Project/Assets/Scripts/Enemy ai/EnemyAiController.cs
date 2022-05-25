using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public enum State { MOVING, CHASE, AIMING, ATTACKING, COOLDOWN, STUNNED };
    private State state = State.MOVING;
    // Update is called once per frame
    public void setState(int changeState)
    {
        state = (State) changeState;
    }
    public State currentState()
    {
        return state;
    }
    public void Die()
    {
        Destroy(gameObject, 0.5f);
    }
    public IEnumerator HitFlash(SpriteRenderer sprite , Color32 originalColor)
    {
        setState(5);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        sprite.color = originalColor;

        setState(0);
    }
    public IEnumerator CooldownAttack(float cooldownTime, int newStateIndex)
    {
        setState(4);//COOLDOWN
        yield return new WaitForSeconds(cooldownTime);

        setState(newStateIndex);// state set based on enemy data
    }
    public IEnumerator Immunity(float immunityTime, bool value)
    {
        yield return new WaitForSeconds(immunityTime);
        value = false;
    }
}