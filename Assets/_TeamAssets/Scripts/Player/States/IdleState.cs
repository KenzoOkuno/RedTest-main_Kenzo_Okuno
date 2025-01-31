using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    #region Variables
    private PlayerController playerController;
    private MovingState movingState;
    private PlayerAnimationController animationController;
    #endregion

    #region Initialization
    private void Awake()
    {
        // Obtém o PlayerController, MovingState e PlayerAnimationController do mesmo GameObject
        playerController = GetComponent<PlayerController>();
        movingState = GetComponent<MovingState>();
        animationController = GetComponent<PlayerAnimationController>();

        // Verifica se o PlayerController foi encontrado
        if (playerController == null)
        {
            Debug.LogError("PlayerController não encontrado no GameObject.");
        }
    }
    #endregion

    #region State Entry
    public override void EnterState()
    {
        // Lógica para quando entrar no estado de Idle (ex: animação de idle)
       
    }
    #endregion

    #region Movement Handling
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

        // Detecta pressionamento de agachar
        if (playerController.crouch.WasPressedThisFrame())
        {
            movingState.ToggleCrouch();
        }

        // Verifica se o input de movimento está acima de um determinado limite
        if (input.magnitude >= 0.1f)
        {
            // Se houver input de movimento, transita para o estado Moving
            GetComponent<PlayerStateMachine>().ChangeState(GetComponent<PlayerStateMachine>().movingState);
        }

        // Verifica se o jogador pressionou o botão de pulo e se está no chão
        if (playerController.jumpAction.WasPressedThisFrame() && movingState.canJump && playerController.IsGrounded())
        {
            movingState.Jump(); // Executa o pulo
            animationController.ActivateBoolJump(); // Ativa a animação de pulo
        }

        playerController.ApplyGravity(); // Aplica a gravidade
    }
    #endregion
}
