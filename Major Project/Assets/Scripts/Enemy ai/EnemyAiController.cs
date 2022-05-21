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
}