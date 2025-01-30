using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // Lista de prefabs dos inimigos
    public Image fadeImage; // Imagem usada para o efeito de fade
    public float fadeDuration = 1.5f;
    public float blackScreenTime = 1.0f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Vector3[] enemyPositions;

    void Start()
    {
        // Salva as posições iniciais para respawn sem instanciar novos inimigos
        enemyPositions = new Vector3[enemyPrefabs.Count];
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyPositions[i] = enemyPrefabs[i].transform.position;
            activeEnemies.Add(enemyPrefabs[i]); // Adiciona os inimigos já existentes
        }
    }

    void Update()
    {
        // Verifica se todos os inimigos foram destruídos
        if (AllEnemiesDestroyed())
        {
            StartCoroutine(RespawnSequence());
        }
    }

    bool AllEnemiesDestroyed()
    {
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
                return false;
        }
        return true;
    }

    IEnumerator RespawnSequence()
    {
        // Inicia o fade para preto
        yield return StartCoroutine(FadeToBlack());

        // Aguarda um curto tempo com a tela preta
        yield return new WaitForSeconds(blackScreenTime);

        // Respawna os inimigos
        RespawnEnemies();

        // Faz o fade para a tela normal
        yield return StartCoroutine(FadeFromBlack());
    }

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

    void RespawnEnemies()
    {
        activeEnemies.Clear();
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[i], enemyPositions[i], Quaternion.identity);
            activeEnemies.Add(newEnemy);
        }
    }
}