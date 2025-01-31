using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MovingState : PlayerState
{
    #region Variables
    private PlayerController playerController;
    private PlayerAnimationController animationController;
    private float currentSpeed;
    private bool isJumping = false;
    public bool canJump = true;
    private float jumpCooldown = 2f;
    private bool isCrouching = false;
    private float crouchHeight = 0.5f;
    private float standHeight = 2f;
    private float crouchSpeed = 1.5f;
    private bool canMoveWhileCrouching = false;
    public float jumpForce = 5f;
    #endregion

    #region Initialization
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    public override void EnterState()
    {
        // Lógica para quando entrar no estado de movimento (ex: animação de corrida)
    }
    #endregion

    #region Movement Handling
    public override void HandleMovement()
    {
        Vector2 input = playerController.GetMovementInput();

        if (playerController.crouch.WasPressedThisFrame()) ToggleCrouch();

        if (isCrouching && !canMoveWhileCrouching) return;

        if (input.magnitude >= 0.1f)
        {
            MovePlayer(input);
        }
        else
        {
            // Muda para o estado de idle caso não haja movimento
            var stateMachine = GetComponent<PlayerStateMachine>();
            stateMachine.ChangeState(stateMachine.idleState);
        }

        playerController.ApplyGravity();
    }

    private void MovePlayer(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;
        currentSpeed = playerController.IsSprinting()
            ? playerController.sprintSpeed
            : (isCrouching ? crouchSpeed : playerController.walkSpeed);

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerController.cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref playerController.rotationSpeed, 0.1f);
        transform.rotation = Quaternion.Euler(0, angle, 0);

        playerController.characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }
    #endregion

    #region Jumping
    public void Jump()
    {
        if (!canJump || isJumping || !playerController.IsGrounded()) return;

        isJumping = true;
        canJump = false;

        playerController.velocity.y = Mathf.Sqrt(jumpForce * -2f * playerController.gravity);

        StartCoroutine(JumpCooldown());
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;

        // Atualiza a UI de cooldown se o objeto for encontrado
        FindAnyObjectByType<JumpCooldownUI>()?.StartCooldownUI(jumpCooldown);

        yield return new WaitForSeconds(jumpCooldown);

        canJump = true;
        isJumping = false;
    }
    #endregion

    #region Crouching
    public void ToggleCrouch()
    {
        if (isCrouching) StandUp();
        else CrouchDown();
    }

    private void CrouchDown()
    {
        isCrouching = true;
        playerController.characterController.height = crouchHeight;
        playerController.characterController.center = new Vector3(0, crouchHeight / 2, 0);
        canMoveWhileCrouching = false;
        animationController.ActivateBoolCrouch();
    }

    private void StandUp()
    {
        if (!isCrouching) return;

        isCrouching = false;
        playerController.characterController.height = standHeight;
        playerController.characterController.center = Vector3.zero;
        canMoveWhileCrouching = true;
        animationController.DeactivateBoolCrouch();
        playerController.velocity.y = 0;
    }
    #endregion

    #region Getter
    public float GetCurrentSpeed() => currentSpeed;
    #endregion
}
