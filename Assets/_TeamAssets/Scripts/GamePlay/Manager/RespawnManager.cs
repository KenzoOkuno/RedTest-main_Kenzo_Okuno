using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    #region Variáveis Públicas
    public List<GameObject> enemyPrefabs; // Lista de prefabs dos inimigos
    public Image fadeImage; // Imagem usada para o efeito de fade
    public float fadeDuration = 1.5f;
    public float blackScreenTime = 1.0f;
    #endregion

    #region Variáveis Privadas
    private Vector3[] enemyPositions; // Posições iniciais dos inimigos
    private bool isRespawning = false; // Controle de estado do respawn
    #endregion

    #region Métodos Unity
    private void Start()
    {
        // Armazena as posições iniciais dos inimigos
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
        // Verifica se todos os inimigos foram destruídos e inicia o respawn
        if (!isRespawning && AllEnemiesDestroyed())
        {
            StartCoroutine(RespawnSequence());
        }
    }
    #endregion

    #region Verificação de Inimigos
    /// <summary>
    /// Verifica se todos os inimigos foram destruídos na cena.
    /// </summary>
    /// <returns>Retorna true se não houver mais inimigos.</returns>
    private bool AllEnemiesDestroyed()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }
    #endregion

    #region Sequência de Respawn
    /// <summary>
    /// Executa a sequência de respawn com fade in/out.
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
    /// Faz a transição para a tela preta.
    /// </summary>
    private IEnumerator FadeToBlack()
    {
        yield return Fade(0, 1);
    }

    /// <summary>
    /// Faz a transição da tela preta para a tela normal.
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
    /// Respawna os inimigos na posição original.
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
