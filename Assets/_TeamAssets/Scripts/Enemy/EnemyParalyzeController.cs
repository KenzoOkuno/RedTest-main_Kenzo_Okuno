using UnityEngine;
using System.Collections;

public class EnemyParalyzeController : MonoBehaviour
{
    #region Variables
    private bool isParalyzed = false; // Estado de paralisia do inimigo
    private float paralyzeDuration = 1f; // Dura��o da paralisia
    private Coroutine paralyzeCoroutine; // Refer�ncia � coroutine da paralisia
    #endregion

    #region Paralyze Logic
    public void Paralyze()
    {
        // Inicia a paralisia se n�o estiver paralisado
        if (!isParalyzed)
        {
            isParalyzed = true;
            if (paralyzeCoroutine != null)
                StopCoroutine(paralyzeCoroutine); // Para a coroutine anterior, se houver

            paralyzeCoroutine = StartCoroutine(ParalyzeDuration()); // Inicia a nova coroutine
        }
    }

    private IEnumerator ParalyzeDuration()
    {
        // Aguarda pela dura��o da paralisia
        yield return new WaitForSeconds(paralyzeDuration);
        isParalyzed = false; // Desfaz a paralisia ap�s o tempo
    }
    #endregion

    #region State Query
    public bool IsParalyzed()
    {
        // Retorna se o inimigo est� paralisado
        return isParalyzed;
    }
    #endregion
}
