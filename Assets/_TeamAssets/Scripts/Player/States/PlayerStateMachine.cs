using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
    public IdleState idleState;
    public MovingState movingState;
    public AttackState attackState;
    

    private void Start()
    {
        // Começa no estado Idle
        ChangeState(idleState);
    }

    public void ChangeState(PlayerState newState)
    {
        Debug.Log("Mudando para estado: " + newState.GetType().Name);
        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState?.EnterState();
    }
}
