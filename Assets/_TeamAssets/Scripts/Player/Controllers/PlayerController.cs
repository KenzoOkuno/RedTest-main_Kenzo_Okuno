using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Configurações de Movimento
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = 1f;
    #endregion

    #region Referências de Áudio
    public AudioSource audioSource;
    public AudioClip punchSound;
    public AudioClip specialPunchSound;
    #endregion

    #region Referências Gerais
    public Transform cameraTransform;
    public CharacterController characterController;
    public Vector3 velocity;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private InputAction attackAction2;
    private InputAction specialAttackAction;
    public InputAction jumpAction;
    public InputAction crouch;

    private PlayerStateMachine stateMachine;
    private PlayerComboController comboController;
    private EnemyParalyzeController enemyParalyzeController;
    #endregion

    #region Métodos Unity
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        stateMachine = GetComponent<PlayerStateMachine>();
        comboController = GetComponent<PlayerComboController>();
        enemyParalyzeController = GetComponent<EnemyParalyzeController>();

        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            sprintAction = playerInput.actions["Sprint"];
            attackAction = playerInput.actions["Attack"];
            attackAction2 = playerInput.actions["Attack2"];
            specialAttackAction = playerInput.actions["SpecialAttack"];
            jumpAction = playerInput.actions["Jump"];
            crouch = playerInput.actions["Crouch"];
        }
    }

    private void Update()
    {
        stateMachine?.CurrentState?.HandleMovement();
        ApplyGravity();
    }
    #endregion

    #region Mecânicas do Jogador
    public void ApplyGravity()
    {
        velocity.y = IsGrounded() && velocity.y < 0 ? -2f : velocity.y + gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public Vector2 GetMovementInput() => moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
    public bool IsSprinting() => sprintAction?.IsPressed() ?? false;
    public bool IsGrounded() => characterController.isGrounded;
    #endregion

    #region Áudio
    public void PlayPunchSound()
    {
        if (audioSource != null && punchSound != null)
            audioSource.PlayOneShot(punchSound);
    }

    public void PlaySpecialPunchSound()
    {
        if (audioSource != null && specialPunchSound != null)
            audioSource.PlayOneShot(specialPunchSound);
    }
    #endregion
}