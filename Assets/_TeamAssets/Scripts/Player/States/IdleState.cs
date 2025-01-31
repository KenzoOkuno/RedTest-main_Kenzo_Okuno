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
        // Obt�m o PlayerController, MovingState e PlayerAnimationController do mesmo GameObject
        playerController = GetComponent<PlayerController>();
        movingState = GetComponent<MovingState>();
        animationController = GetComponent<PlayerAnimationController>();

        // Verifica se o PlayerController foi encontrado
        if (playerController == null)
        {
            Debug.LogError("PlayerController n�o encontrado no GameObject.");
        }
    }
    #endregion

    #region State Entry
    public override void EnterState()
    {
        // L�gica para quando entrar no estado de Idle (ex: anima��o de idle)
       
    }
    #endregion

    #region Movement Handling
    public override void HandleMovement()
    {
        // Verifica se o playerController foi atribu�do corretamente
        if (playerController == null)
        {
            Debug.LogError("PlayerController n�o est� atribu�do no IdleState.");
            return;
        }

        // Obt�m os valores de input para o movimento
        Vector2 input = playerController.GetMovementInput();

        // Detecta pressionamento de agachar
        if (playerController.crouch.WasPressedThisFrame())
        {
            movingState.ToggleCrouch();
        }

        // Verifica se o input de movimento est� acima de um determinado limite
        if (input.magnitude >= 0.1f)
        {
            // Se houver input de movimento, transita para o estado Moving
            GetComponent<PlayerStateMachine>().ChangeState(GetComponent<PlayerStateMachine>().movingState);
        }

        // Verifica se o jogador pressionou o bot�o de pulo e se est� no ch�o
        if (playerController.jumpAction.WasPressedThisFrame() && movingState.canJump && playerController.IsGrounded())
        {
            movingState.Jump(); // Executa o pulo
            animationController.ActivateBoolJump(); // Ativa a anima��o de pulo
        }

        playerController.ApplyGravity(); // Aplica a gravidade
    }
    #endregion
}
