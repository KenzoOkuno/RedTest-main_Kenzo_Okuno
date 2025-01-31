using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    #region Singleton
    public static CameraShake Instance; // Inst�ncia singleton para acesso global
    #endregion

    #region Variables
    private Vector3 originalPosition; // Posi��o original da c�mera
    #endregion

    #region Initialization
    private void Awake()
    {
        // Inicializa a inst�ncia singleton
        Instance = this;
    }
    #endregion

    #region Shake Logic
    public void Shake(float duration, float magnitude)
    {
        // Inicia o efeito de shake com a dura��o e magnitude especificadas
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        // Armazena a posi��o original da c�mera para restaurar ap�s o shake
        originalPosition = transform.localPosition;
        float elapsed = 0f;

        // Realiza o shake enquanto o tempo n�o acabar
        while (elapsed < duration)
        {
            // Gera um deslocamento aleat�rio nos eixos X e Y
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica o shake na posi��o local da c�mera
            transform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Restaura a posi��o original da c�mera
        transform.localPosition = originalPosition;
    }
    #endregion
}
