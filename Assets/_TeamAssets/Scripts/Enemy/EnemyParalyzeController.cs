using UnityEngine;
using System.Collections;

public class EnemyParalyzeController : MonoBehaviour
{
    #region Variables
    private bool isParalyzed = false; // Estado de paralisia do inimigo
    private float paralyzeDuration = 1f; // Duração da paralisia
    private Coroutine paralyzeCoroutine; // Referência à coroutine da paralisia
    #endregion

    #region Paralyze Logic
    public void Paralyze()
    {
        // Inicia a paralisia se não estiver paralisado
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
        // Aguarda pela duração da paralisia
        yield return new WaitForSeconds(paralyzeDuration);
        isParalyzed = false; // Desfaz a paralisia após o tempo
    }
    #endregion

    #region State Query
    public bool IsParalyzed()
    {
        // Retorna se o inimigo está paralisado
        return isParalyzed;
    }
    #endregion
}
