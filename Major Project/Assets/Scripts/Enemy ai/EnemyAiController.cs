using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiController : MonoBehaviour
{
    public enum State { MOVING, CHASE, AIMING, ATTACKING, COOLDOWN, STUNNED };
    private State state = State.MOVING;
    // Start is called before the first frame update
    // Update is called once per frame
    public void states()
    {
        switch (state)
        {
            case State.MOVING:
                break;
            case State.CHASE:
                break;
            case State.AIMING:
                break;
            case State.ATTACKING:
                break;
            case State.COOLDOWN:
                break;
            case State.STUNNED:
                break;
        }
    }
    public void setState(int changeState)
    {
        state = (State) changeState;
    }
    public State currentState()
    {
        return state;
    }
}