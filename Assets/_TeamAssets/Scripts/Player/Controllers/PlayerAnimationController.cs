using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerStateMachine stateMachine;
    private AttackState attackState;

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

    /// <summary>
    /// Gatilho para iniciar um ataque com base no combo atual.
    /// </summary>
    /// <param name="comboStep">Passo do combo (1, 2 ou 3).</param>
    public void TriggerAttack(int comboStep)
    {
        // Reseta triggers antigos para evitar conflitos
        ResetAttackTriggers();

        switch (comboStep)
        {
            case 1:
                animator.SetTrigger("Attack1");
              //  attackState.ActivateCollider();
                animator.SetBool("IsAttacking", true);
                break;
            case 2:
                animator.SetTrigger("Attack2");
               // attackState.ActivateCollider();
                animator.SetBool("IsAttacking", true);
                break;
            case 3:
                animator.SetTrigger("Attack3");
              //  attackState.ActivateCollider();
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

    /// <summary>
    /// Reseta o estado do Animator ao iniciar ou após uma sequência de ataques.
    /// </summary>
    private void ResetAnimatorState()
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("SpecialAttack");

    }

    /// <summary>
    /// Reseta os triggers de ataque para garantir que apenas o trigger correto seja usado.
    /// </summary>
    private void ResetAttackTriggers()
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("SpecialAttack");
    }

    /// <summary>
    /// Método chamado ao final de uma animação de ataque para redefinir o estado de ataque.
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        animator.SetBool("IsAttacking", false);
        attackState.DeactivateCollider();
        attackState.DeactivateSpecialAttackCollider();
        ResetAnimatorState();
    }
    
    public void ActivateBoolJump()
    {
        animator.SetBool("Jump",true);
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
}
