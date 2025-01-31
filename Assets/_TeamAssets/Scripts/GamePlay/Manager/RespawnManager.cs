using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    #region Public Variables
    public List<GameObject> enemyPrefabs; // Lista de prefabs dos inimigos
    public Image fadeImage; // Imagem usada para o efeito de fade
    public float fadeDuration = 1.5f; // Duração do efeito de fade
    public float blackScreenTime = 1.0f; // Tempo com a tela preta após o fade
    #endregion

    #region Private Variables
    private Vector3[] enemyPositions; // Posições iniciais dos inimigos
    private bool isRespawning = false; // Flag que controla o processo de respawn
    #endregion

    #region Unity Lifecycle
    void Start()
    {
        // Salva as posições iniciais dos inimigos para o respawn
        enemyPositions = new Vector3[enemyPrefabs.Count];
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyPositions[i] = enemyPrefabs[i].transform.position;
        }
    }

    void Update()
    {
        // Verifica se todos os inimigos foram destruídos e não está já respawnando
        if (!isRespawning && AllEnemiesDestroyed())
        {
            StartCoroutine(RespawnSequence());
        }
    }
    #endregion

    #region Helper Methods
    bool AllEnemiesDestroyed()
    {
        // Conta os inimigos na cena pela tag "Enemy"
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    void RespawnEnemies()
    {
        // Respawna os inimigos nas suas posições iniciais
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (enemyPrefabs[i] != null)
            {
                Instantiate(enemyPrefabs[i], enemyPositions[i], Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab do inimigo está faltando na lista!");
            }
        }
    }
    #endregion

    #region Respawn Sequence
    IEnumerator RespawnSequence()
    {
        isRespawning = true;

        // Inicia o fade para preto
        yield return StartCoroutine(FadeToBlack());

        // Aguarda um curto tempo com a tela preta
        yield return new WaitForSeconds(blackScreenTime);

        // Respawna os inimigos
        RespawnEnemies();

        // Faz o fade para a tela normal
        yield return StartCoroutine(FadeFromBlack());

        isRespawning = false;
    }
    #endregion

    #region Fade Effects
    IEnumerator FadeToBlack()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }
    #endregion
}
