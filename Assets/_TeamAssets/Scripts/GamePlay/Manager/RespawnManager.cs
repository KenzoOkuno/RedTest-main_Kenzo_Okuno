using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    #region Vari�veis P�blicas
    public List<GameObject> enemyPrefabs; // Lista de prefabs dos inimigos
    public Image fadeImage; // Imagem usada para o efeito de fade
    public float fadeDuration = 1.5f;
    public float blackScreenTime = 1.0f;
    #endregion

    #region Vari�veis Privadas
    private Vector3[] enemyPositions; // Posi��es iniciais dos inimigos
    private bool isRespawning = false; // Controle de estado do respawn
    #endregion

    #region M�todos Unity
    private void Start()
    {
        // Armazena as posi��es iniciais dos inimigos
        enemyPositions = new Vector3[enemyPrefabs.Count];
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (enemyPrefabs[i] != null)
            {
                enemyPositions[i] = enemyPrefabs[i].transform.position;
            }
        }
    }

    private void Update()
    {
        // Verifica se todos os inimigos foram destru�dos e inicia o respawn
        if (!isRespawning && AllEnemiesDestroyed())
        {
            StartCoroutine(RespawnSequence());
        }
    }
    #endregion

    #region Verifica��o de Inimigos
    /// <summary>
    /// Verifica se todos os inimigos foram destru�dos na cena.
    /// </summary>
    /// <returns>Retorna true se n�o houver mais inimigos.</returns>
    private bool AllEnemiesDestroyed()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }
    #endregion

    #region Sequ�ncia de Respawn
    /// <summary>
    /// Executa a sequ�ncia de respawn com fade in/out.
    /// </summary>
    private IEnumerator RespawnSequence()
    {
        isRespawning = true;

        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(blackScreenTime);
        RespawnEnemies();
        yield return StartCoroutine(FadeFromBlack());

        isRespawning = false;
    }
    #endregion

    #region Efeitos de Fade
    /// <summary>
    /// Faz a transi��o para a tela preta.
    /// </summary>
    private IEnumerator FadeToBlack()
    {
        yield return Fade(0, 1);
    }

    /// <summary>
    /// Faz a transi��o da tela preta para a tela normal.
    /// </summary>
    private IEnumerator FadeFromBlack()
    {
        yield return Fade(1, 0);
    }

    /// <summary>
    /// Controla o efeito de fade para preto ou normal.
    /// </summary>
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, endAlpha);
    }
    #endregion

    #region Respawn de Inimigos
    /// <summary>
    /// Respawna os inimigos na posi��o original.
    /// </summary>
    private void RespawnEnemies()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (enemyPrefabs[i] != null)
            {
                Instantiate(enemyPrefabs[i], enemyPositions[i], Quaternion.identity);
            }
        }
    }
    #endregion
}
