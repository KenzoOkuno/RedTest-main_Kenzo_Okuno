using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    #region Singleton
    public static CameraShake Instance; // Instância singleton para acesso global
    #endregion

    #region Variables
    private Vector3 originalPosition; // Posição original da câmera
    #endregion

    #region Initialization
    private void Awake()
    {
        // Inicializa a instância singleton
        Instance = this;
    }
    #endregion

    #region Shake Logic
    public void Shake(float duration, float magnitude)
    {
        // Inicia o efeito de shake com a duração e magnitude especificadas
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        // Armazena a posição original da câmera para restaurar após o shake
        originalPosition = transform.localPosition;
        float elapsed = 0f;

        // Realiza o shake enquanto o tempo não acabar
        while (elapsed < duration)
        {
            // Gera um deslocamento aleatório nos eixos X e Y
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica o shake na posição local da câmera
            transform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Restaura a posição original da câmera
        transform.localPosition = originalPosition;
    }
    #endregion
}
