using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : MonoBehaviour
{
    #region Variáveis Privadas
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private const float comboResetTime = 1f;
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
    private const float specialAttackCooldown = 5f;
    private float lastSpecialAttackTime = 0f;

    public PlayerComboController comboController;
    public EnemyParalyzeController enemyParalyzeController;
    #endregion

    #region Métodos Unity
    private void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        attackInput.action.performed += ctx => HandleAttack(ctx, true);
        attackInput2.action.performed += ctx => HandleAttack(ctx, false);
        specialAttackInput.action.performed += HandleSpecialAttack;
    }

    private void OnDisable()
    {
        attackInput.action.performed -= ctx => HandleAttack(ctx, true);
        attackInput2.action.performed -= ctx => HandleAttack(ctx, false);
        specialAttackInput.action.performed -= HandleSpecialAttack;
    }
    #endregion

    #region Manipulação de Ataques
    private void HandleAttack(InputAction.CallbackContext context, bool isPrimary)
    {
        if (Time.time - lastAttackTime > comboResetTime)
            comboStep = 0;

        comboStep = (comboStep >= 3) ? 0 : comboStep + 1;
        lastAttackTime = Time.time;

        if (enemyParalyzeController.IsParalyzed())
            comboController.ContinueCombo();
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
    #endregion

    #region Controle de Colisores
    public void ActivateCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.SetActive(true);
            playerController.PlayPunchSound();
            Invoke(nameof(DeactivateCollider), colliderDuration);
        }
    }

    public void ActivateSpecialAttackCollider()
    {
        if (SpecialAttackCollider != null)
        {
            SpecialAttackCollider.SetActive(true);
            playerController.PlaySpecialPunchSound();
            Invoke(nameof(DeactivateSpecialAttackCollider), colliderDuration);
        }
    }

    public void DeactivateCollider() => attackCollider?.SetActive(false);
    public void DeactivateSpecialAttackCollider() => SpecialAttackCollider?.SetActive(false);
    #endregion

    #region Atualização
    private void Update() => comboController.UpdateCombo();
    #endregion
}