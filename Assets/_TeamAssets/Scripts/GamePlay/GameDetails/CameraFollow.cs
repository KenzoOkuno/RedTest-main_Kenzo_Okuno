using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Target Settings
    [Header("Target Settings")]
    public Transform player; // Refer�ncia ao transform do jogador
    #endregion

    #region Camera Offset
    [Header("Camera Offset")]
    [SerializeField] private float offsetX = 0f; // Deslocamento horizontal em rela��o ao jogador
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidade de suaviza��o do movimento
    #endregion

    #region Camera Position Variables
    private float fixedY; // Posi��o fixa no eixo Y
    private float fixedZ; // Posi��o fixa no eixo Z
    #endregion

    #region Initialization
    private void Start()
    {
        // Salva as posi��es Y e Z iniciais da c�mera para mant�-las fixas
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }
    #endregion

    #region Update Logic
    private void LateUpdate()
    {
        // Verifica se o jogador est� atribu�do � c�mera
        if (player == null)
        {
            Debug.LogWarning("Player n�o foi atribu�do � c�mera.");
            return;
        }

        // Calcula a nova posi��o no eixo X, mantendo as posi��es Y e Z fixas
        Vector3 targetPosition = new Vector3(player.position.x + offsetX, fixedY, fixedZ);

        // Suaviza a transi��o entre a posi��o atual e a posi��o-alvo
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Atualiza a posi��o da c�mera
        transform.position = smoothedPosition;
    }
    #endregion
}
