using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    #region Variables
    private Animator animator;
    private EnemyController enemyController;
    private bool isPunched = false; // Controla se o inimigo est� "Punched"
    #endregion

    #region Initialization
    private void Awake()
    {
        // Obt�m o componente Animator e EnemyController do inimigo
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }
    #endregion

    #region Damage Handling
    // Chamar este m�todo quando o inimigo receber dano
    public void TakeDamage()
    {
        if (!isPunched) // Garante que o estado Punched n�o ser� ativado repetidamente
        {
            isPunched = true;
            animator.SetBool("IsPunched", true); // Ativa o estado Punched
            StartCoroutine(ResetToIdle());
        }
    }

    // Retorna para o estado Idle ap�s o t�rmino da anima��o Punched
    private System.Collections.IEnumerator ResetToIdle()
    {
        // Aguarda o tempo necess�rio para completar a anima��o atual
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isPunched = false;
        animator.SetBool("IsPunched", false); // Retorna ao estado Idle
    }
    #endregion

    #region Death Handling
    // Chama este m�todo para iniciar a anima��o de morte
    public void Die()
    {
        
        animator.SetTrigger("Die"); // Ativa o trigger de morte no Animator
    }
    #endregion
}
