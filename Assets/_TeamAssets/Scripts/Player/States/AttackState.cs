using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : MonoBehaviour
{
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private float comboResetTime = 1f;
    private Animator animator;
    private PlayerAnimationController playerAnimationController;
    public GameObject attackCollider;
    public GameObject SpecialAttackCollider;
    public PlayerController playerController;
    private PlayerEnergyController playerEnergyController;
    [SerializeField] private float colliderDuration = 0.2f;
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private InputActionReference attackInput2;
    [SerializeField] private InputActionReference specialAttackInput;
    public GameObject Testeç;

    private float specialAttackCooldown = 5f;
    private float lastSpecialAttackTime = 0f;

    public PlayerComboController comboController;
    public EnemyParalyzeController enemyParalyzeController;

    private void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        attackInput.action.performed += context => HandleAttack(context, true);
        attackInput2.action.performed += context => HandleAttack(context, false);
        specialAttackInput.action.performed += HandleSpecialAttack;
    }

    private void OnDisable()
    {
        attackInput.action.performed -= context => HandleAttack(context, true);
        attackInput2.action.performed -= context => HandleAttack(context, false);
        specialAttackInput.action.performed -= HandleSpecialAttack;
    }

    private void HandleAttack(InputAction.CallbackContext context, bool isPrimary)
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        if (comboStep >= 3)
        {
            comboStep = 0;
        }

        comboStep++;
        lastAttackTime = Time.time;

        if (enemyParalyzeController.IsParalyzed())
        {
            comboController.ContinueCombo();
        }
        else
        {
            comboController.StartCombo();
            enemyParalyzeController.Paralyze();
        }

        playerAnimationController.TriggerAttack(comboStep);
        animator.SetTrigger(isPrimary ? "Mouse1" : "Mouse2");
    }

    private void HandleSpecialAttack(InputAction.CallbackContext context)
    {
        if (Time.time - lastSpecialAttackTime >= specialAttackCooldown)
        {
            lastSpecialAttackTime = Time.time;
            playerAnimationController.TriggerSpecialAttack();
        }
    }

    public void ActivateCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.SetActive(true);
            playerController.PlayPunchSound();
            Invoke("DeactivateCollider", colliderDuration);
        }
    }

    public void ActivateSpecialAttackCollider()
    {
        if (SpecialAttackCollider != null)
        {
            SpecialAttackCollider.SetActive(true);
            playerController.PlaySpecialPunchSound();
            Invoke("DeactivateSpecialAttackCollider", colliderDuration);
        }
    }

    public void DeactivateCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.SetActive(false);
        }
    }

    public void DeactivateSpecialAttackCollider()
    {
        if (SpecialAttackCollider != null)
        {
            SpecialAttackCollider.SetActive(false);
        }
    }

    private void Update()
    {
        comboController.UpdateCombo();
    }
}
