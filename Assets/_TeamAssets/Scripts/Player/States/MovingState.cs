using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MovingState : PlayerState
{
    private PlayerController playerController;
    private PlayerAnimationController animationController;
    private float currentSpeed; // Armazena a velocidade atual
    private bool isJumping = false;  // Flag para indicar se o personagem está pulando
    public bool canJump = true;     // Controle de cooldown para o pulo
    private float jumpCooldown = 2f; // Cooldown do pulo
    private bool isCrouching = false;  // Flag para verificar se o personagem está agachado
    private float crouchHeight = 0.5f;  // Altura do personagem agachado
    private float standHeight = 2f;    // Altura do personagem em pé
    private float crouchSpeed = 1.5f;  // Velocidade ao agachar
    private float normalSpeed;         // Velocidade normal
    private bool canMoveWhileCrouching = false;  // Flag que define se pode ou não mover enquanto agachado

    public float jumpForce = 5f;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    public override void EnterState()
    {
        // Lógica para quando entrar no estado de movimento (ex: animação de corrida)
    }

    public override void HandleMovement()
    {
        Vector2 input = playerController.GetMovementInput();

        // Lógica de agachamento
        if (playerController.crouch.WasPressedThisFrame()) // Usando o InputSystem para detectar pressionamento de agachar
        {
            ToggleCrouch();

        }

        // Se o personagem estiver agachado, não pode mover
        if (isCrouching && !canMoveWhileCrouching)
        {
            return;
        }

        // Movimento normal
        if (input.magnitude >= 0.1f)
        {
            MovePlayer(input);
        }
        else
        {
            // Se não houver movimento, volta para o estado Idle
            GetComponent<PlayerStateMachine>().ChangeState(GetComponent<PlayerStateMachine>().idleState);
        }

        // Lógica de pulo


        playerController.ApplyGravity();
    }

    private void MovePlayer(Vector2 input)
    {
        // Determina a direção do movimento
        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;

        // Atualiza a velocidade com base no sprint
        currentSpeed = playerController.IsSprinting() ? playerController.sprintSpeed : (isCrouching ? crouchSpeed : playerController.walkSpeed);

        // Calcula o ângulo para rotação do personagem
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerController.cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref playerController.rotationSpeed, 0.1f);

        // Aplica a rotação
        transform.rotation = Quaternion.Euler(0, angle, 0);
        // Move o personagem
        playerController.characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        if (!canJump || isJumping || !playerController.IsGrounded()) return;

        isJumping = true;
        canJump = false;

        Debug.Log("[JUMP] Pulo iniciado!");

        // Definir a velocidade vertical corretamente
        playerController.velocity.y = Mathf.Sqrt(jumpForce * -2f * playerController.gravity);

        StartCoroutine(JumpCooldown());
    }


    private IEnumerator JumpCooldown()
    {
        canJump = false;

        // Obtém a UI e inicia a contagem regressiva
        JumpCooldownUI cooldownUI = FindAnyObjectByType<JumpCooldownUI>();
        if (cooldownUI != null)
        {
            cooldownUI.StartCooldownUI(jumpCooldown);
        }

        yield return new WaitForSeconds(jumpCooldown);

        canJump = true;
        isJumping = false;
    }


    public void ToggleCrouch()
    {
        // Alterna entre agachar e levantar
        if (isCrouching)
        {
            StandUp();
        }
        else
        {
            CrouchDown();

        }
    }

    private void CrouchDown()
    {
        isCrouching = true;
        playerController.characterController.height = crouchHeight;
        playerController.characterController.center = new Vector3(0, crouchHeight / 2, 0);
        canMoveWhileCrouching = false;  // Não permite movimentação enquanto agachado
        animationController.ActivateBoolCrouch();
    }

    private void StandUp()
    {
        if (!isCrouching) return;

        isCrouching = false;

        // Ajusta apenas a altura, mantendo o centro correto
        playerController.characterController.height = standHeight;
        playerController.characterController.center = Vector3.zero; // Garante que o centro não desloque para cima

        canMoveWhileCrouching = true;
        animationController.DeactivateBoolCrouch();

        // Zerar a velocidade vertical para evitar queda abrupta
        playerController.velocity.y = 0;
    }






    public float GetCurrentSpeed()
    {
        // Retorna a velocidade atual (normal ou enquanto agachado)
        return currentSpeed;
    }
}