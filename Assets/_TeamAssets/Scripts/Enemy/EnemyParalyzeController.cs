using UnityEngine;
using System.Collections;

public class EnemyParalyzeController : MonoBehaviour
{
    #region Variáveis Privadas
    private bool isParalyzed = false; // Indica se o inimigo está paralisado
    private float paralyzeDuration = 1f; // Duração da paralisia em segundos
    private Coroutine paralyzeCoroutine; // Referência à coroutine de paralisia
    #endregion

    #region Métodos Públicos
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

            // Inicia a coroutine para controlar a duração da paralisia
            paralyzeCoroutine = StartCoroutine(ParalyzeDuration());
        }
    }

    /// <summary>
    /// Verifica se o inimigo está paralisado.
    /// </summary>
    public bool IsParalyzed()
    {
        return isParalyzed;
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Coroutine que controla a duração da paralisia.
    /// </summary>
    private IEnumerator ParalyzeDuration()
    {
        yield return new WaitForSeconds(paralyzeDuration);
        isParalyzed = false;
    }
    #endregion
}
