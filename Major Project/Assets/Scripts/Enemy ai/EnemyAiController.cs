using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiController : MonoBehaviour
{
    public enum State { MOVING, CHASE, AIMING, ATTACKING, COOLDOWN, STUNNED };
    private State state = State.MOVING;
    // Start is called before the first frame update
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