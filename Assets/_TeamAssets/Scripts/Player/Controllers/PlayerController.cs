using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = 1f;
    public AudioSource audioSource;
    public AudioClip punchSound;
    public AudioClip specialPunchSound;
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
    private PlayerComboController comboController;  // Referência ao controlador de combo
    private EnemyParalyzeController enemyParalyzeController;  // Controlador de paralisia do inimigo

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        stateMachine = GetComponent<PlayerStateMachine>();
        comboController = GetComponent<PlayerComboController>();  // Inicializa o controlador de combos
        enemyParalyzeController = GetComponent<EnemyParalyzeController>();  // Inicializa o controlador de paralisia

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
        else
        {
            Debug.LogError("PlayerInput não está configurado.");
        }
    }

    private void Update()
    {
        stateMachine?.CurrentState?.HandleMovement();  // Delegar movimento ao estado atual
        ApplyGravity();  // Aplica a gravidade

        // HandleInputActions(); // Processa as ações de entrada
    }




    public void ApplyGravity()
    {
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f; // Manter fixo no chão
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Aplicar gravidade suavemente
        }

        characterController.Move(velocity * Time.deltaTime);
    }


    public Vector2 GetMovementInput()
    {
        return moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
    }

    public bool IsSprinting()
    {
        return sprintAction?.IsPressed() ?? false;
    }

    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }

    public void PlayPunchSound()
    {
        if (audioSource != null && punchSound != null)
        {
            Debug.Log("Tocando som de dano.");
            audioSource.PlayOneShot(punchSound);
        }
        else
        {
            Debug.LogError("Erro: AudioSource ou DamageSound não configurados.");
        }
    }

    public void PlaySpecialPunchSound()
    {
        if (audioSource != null && specialPunchSound != null)
        {
            Debug.Log("Tocando som de dano.");
            audioSource.PlayOneShot(specialPunchSound);
        }
        else
        {
            Debug.LogError("Erro: AudioSource ou DamageSound não configurados.");
        }
    }
}