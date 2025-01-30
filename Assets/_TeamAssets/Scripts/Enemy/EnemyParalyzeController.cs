using UnityEngine;
using System.Collections;

public class EnemyParalyzeController : MonoBehaviour
{
    private bool isParalyzed = false;
    private float paralyzeDuration = 1f; // Duração da paralisia
    private Coroutine paralyzeCoroutine;

    public void Paralyze()
    {
        if (!isParalyzed)
        {
            isParalyzed = true;
            if (paralyzeCoroutine != null)
                StopCoroutine(paralyzeCoroutine);

            paralyzeCoroutine = StartCoroutine(ParalyzeDuration());
        }
    }

    private IEnumerator ParalyzeDuration()
    {
        yield return new WaitForSeconds(paralyzeDuration);
        isParalyzed = false;
    }

    public bool IsParalyzed()
    {
        return isParalyzed;
    }
}
