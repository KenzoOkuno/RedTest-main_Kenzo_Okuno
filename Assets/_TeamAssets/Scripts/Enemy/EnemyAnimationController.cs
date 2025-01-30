using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemyController;
    private bool isPunched = false; // Controla se o inimigo está "Punched"

    private void Awake()
    {
        // Obtém o componente Animator do inimigo
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    // Chamar este método quando o inimigo receber dano
    public void TakeDamage()
    {
        if (!isPunched) // Garante que o estado Punched não será ativado repetidamente
        {
            isPunched = true;
            animator.SetBool("IsPunched", true); // Ativa o estado Punched
            StartCoroutine(ResetToIdle());
        }
    }

    // Retorna para o estado Idle após o término da animação Punched
    private System.Collections.IEnumerator ResetToIdle()
    {
        // Aguarda o tempo necessário para completar a animação atual
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isPunched = false;
        animator.SetBool("IsPunched", false); // Retorna ao estado Idle
    }

    // Chama este método para iniciar a animação de morte
    public void Die()
    {
        Debug.Log("Trigger de Morte ativado!");
        animator.SetTrigger("Die"); // Ativa o trigger de morte no Animator
    }
}