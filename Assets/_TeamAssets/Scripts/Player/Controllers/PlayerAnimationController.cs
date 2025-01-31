using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Variáveis Privadas
    private Animator animator;
    private PlayerStateMachine stateMachine;
    private AttackState attackState;
    #endregion

    #region Métodos Unity
    private void Awake()
    {
        // Obtém os componentes necessários
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<PlayerStateMachine>();
        attackState = GetComponent<AttackState>();
    }

    private void Start() => ResetAnimatorState();

    private void Update() => UpdateSpeedParameter();
    #endregion

    #region Controle de Velocidade
    private void UpdateSpeedParameter()
    {
        float speed = stateMachine is { CurrentState: MovingState movingState } ? movingState.GetCurrentSpeed() : 0f;
        animator.SetFloat("Speed", speed);
    }
    #endregion

    #region Ataques
    /// <summary>
    /// Gatilho para iniciar um ataque com base no combo atual.
    /// </summary>
    public void TriggerAttack(int comboStep)
    {
        // Reseta triggers antigos para evitar conflitos
        ResetAttackTriggers();

        if (comboStep is >= 1 and <= 3)
        {
            animator.SetTrigger($"Attack{comboStep}");
            animator.SetBool("IsAttacking", true);
        }
    }

    public void TriggerSpecialAttack()
    {
        // Reseta triggers antigos para evitar conflitos
        ResetAttackTriggers();

        // Gatilho do ataque especial
        animator.SetTrigger("SpecialAttack");
        animator.SetBool("IsAttacking", true);
    }
    #endregion

    #region Reset de Animação
    private void ResetAnimatorState() => ResetAttackTriggers();

    private void ResetAttackTriggers()
    {
        for (int i = 1; i <= 3; i++)
            animator.ResetTrigger($"Attack{i}");
        animator.ResetTrigger("SpecialAttack");
    }
    #endregion

    #region Eventos de Animação
    public void OnAttackAnimationEnd()
    {
        animator.SetBool("IsAttacking", false);
        attackState.DeactivateCollider();
        attackState.DeactivateSpecialAttackCollider();
        ResetAnimatorState();
    }
    #endregion

    #region Controle de Estados
    public void ActivateBoolJump() => animator.SetBool("Jump", true);
    public void DeactivateBoolJump() => animator.SetBool("Jump", false);
    public void ActivateBoolCrouch() => animator.SetBool("Crouch", true);
    public void DeactivateBoolCrouch() => animator.SetBool("Crouch", false);
    #endregion
}