using UnityEngine;
using System.Collections;

public class EnemyParalyzeController : MonoBehaviour
{
    #region Vari�veis Privadas
    private bool isParalyzed = false; // Indica se o inimigo est� paralisado
    private float paralyzeDuration = 1f; // Dura��o da paralisia em segundos
    private Coroutine paralyzeCoroutine; // Refer�ncia � coroutine de paralisia
    #endregion

    #region M�todos P�blicos
    /// <summary>
    /// Paralisa o inimigo por um tempo determinado.
    /// </summary>
    public void Paralyze()
    {
        if (!isParalyzed)
        {
            isParalyzed = true;

            // Interrompe a coroutine anterior, se existir
            if (paralyzeCoroutine != null)
                StopCoroutine(paralyzeCoroutine);

            // Inicia a coroutine para controlar a dura��o da paralisia
            paralyzeCoroutine = StartCoroutine(ParalyzeDuration());
        }
    }

    /// <summary>
    /// Verifica se o inimigo est� paralisado.
    /// </summary>
    public bool IsParalyzed()
    {
        return isParalyzed;
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Coroutine que controla a dura��o da paralisia.
    /// </summary>
    private IEnumerator ParalyzeDuration()
    {
        yield return new WaitForSeconds(paralyzeDuration);
        isParalyzed = false;
    }
    #endregion
}
