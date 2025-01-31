using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Target Settings
    [Header("Target Settings")]
    public Transform player; // Referência ao transform do jogador
    #endregion

    #region Camera Offset
    [Header("Camera Offset")]
    [SerializeField] private float offsetX = 0f; // Deslocamento horizontal em relação ao jogador
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidade de suavização do movimento
    #endregion

    #region Camera Position Variables
    private float fixedY; // Posição fixa no eixo Y
    private float fixedZ; // Posição fixa no eixo Z
    #endregion

    #region Initialization
    private void Start()
    {
        // Salva as posições Y e Z iniciais da câmera para mantê-las fixas
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }
    #endregion

    #region Update Logic
    private void LateUpdate()
    {
        // Verifica se o jogador está atribuído à câmera
        if (player == null)
        {
            Debug.LogWarning("Player não foi atribuído à câmera.");
            return;
        }

        // Calcula a nova posição no eixo X, mantendo as posições Y e Z fixas
        Vector3 targetPosition = new Vector3(player.position.x + offsetX, fixedY, fixedZ);

        // Suaviza a transição entre a posição atual e a posição-alvo
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;
    }
    #endregion
}
