using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player; // Referência ao transform do jogador

    [Header("Camera Offset")]
    [SerializeField] private float offsetX = 0f; // Deslocamento horizontal em relação ao jogador
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidade de suavização do movimento

    private float fixedY; // Posição fixa no eixo Y
    private float fixedZ; // Posição fixa no eixo Z

    private void Start()
    {
        // Salva as posições Y e Z iniciais da câmera
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player não foi atribuído à câmera.");
            return;
        }

        // Calcula a nova posição no eixo X
        Vector3 targetPosition = new Vector3(player.position.x + offsetX, fixedY, fixedZ);

        // Suaviza a transição entre a posição atual e a posição-alvo
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;
    }
}
