using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    private PlayerController playerController;
    private MovingState movingState;
    private PlayerAnimationController animationController;
    

    private void Awake()
    {
        // Obtém o PlayerController do mesmo GameObject
        playerController = GetComponent<PlayerController>();
        movingState = GetComponent<MovingState>();
        animationController = GetComponent<PlayerAnimationController>();
        
        // Verifica se o PlayerController foi encontrado
        if (playerController == null)
        {
            Debug.LogError("PlayerController não encontrado no GameObject.");
        }
    }

    public override void EnterState()
    {
        // Lógica para quando entrar no estado de Idle (ex: animação de idle)
        Debug.Log("Entrando no estado Idle");
    }

    public override void HandleMovement()
    {
        // Verifica se o playerController foi atribuído corretamente
        if (playerController == null)
        {
            Debug.LogError("PlayerController não está atribuído no IdleState.");
            return;
        }


        // Obtém os valores de input para o movimento
        Vector2 input = playerController.GetMovementInput();

        if (playerController.crouch.WasPressedThisFrame()) // Usando o InputSystem para detectar pressionamento de agachar
        {
            movingState.ToggleCrouch();

        }
        // Verifica se o input de movimento está acima de um determinado limite
        if (input.magnitude >= 0.1f)
        {
            // Se houver input de movimento, transita para o estado Moving
            GetComponent<PlayerStateMachine>().ChangeState(GetComponent<PlayerStateMachine>().movingState);
        }
        
        if (playerController.jumpAction.WasPressedThisFrame() && movingState.canJump && playerController.IsGrounded())
        {
            movingState.Jump();
            animationController.ActivateBoolJump();
        }

        playerController.ApplyGravity();
    }

}

