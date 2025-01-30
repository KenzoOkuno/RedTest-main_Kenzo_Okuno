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

        if (animator == null)
        {
            Debug.LogError("Animator não está atribuído!");
        }
    }

    private void Start()
    {
        // Reseta todos os triggers e estados no início
        ResetAnimatorState();
    }

    private void Update()
    {
        UpdateSpeedParameter();
    }
    #endregion

    #region Controle de Velocidade
    private void UpdateSpeedParameter()
    {
        if (stateMachine != null && stateMachine.CurrentState is MovingState movingState)
        {
            // Obtém a velocidade atual do MovingState
            float speed = movingState.GetCurrentSpeed();
            animator.SetFloat("Speed", speed);
        }
        else
        {
            // Se não estiver no estado de movimento, define Speed como 0
            animator.SetFloat("Speed", 0f);
        }
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

        switch (comboStep)
        {
            case 1:
                animator.SetTrigger("Attack1");
                animator.SetBool("IsAttacking", true);
                break;
            case 2:
                animator.SetTrigger("Attack2");
                animator.SetBool("IsAttacking", true);
                break;
            case 3:
                animator.SetTrigger("Attack3");
                animator.SetBool("IsAttacking", true);
                break;
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
    private void ResetAnimatorState()
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("SpecialAttack");
    }

    private void ResetAttackTriggers()
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
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
    public void ActivateBoolJump()
    {
        animator.SetBool("Jump", true);
    }

    public void DeactivateBoolJump()
    {
        animator.SetBool("Jump", false);
    }

    public void ActivateBoolCrouch()
    {
        animator.SetBool("Crouch", true);
    }

    public void DeactivateBoolCrouch()
    {
        animator.SetBool("Crouch", false);
    }
    #endregion
}
