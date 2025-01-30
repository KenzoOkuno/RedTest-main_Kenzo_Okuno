using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player; // Refer�ncia ao transform do jogador

    [Header("Camera Offset")]
    [SerializeField] private float offsetX = 0f; // Deslocamento horizontal em rela��o ao jogador
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidade de suaviza��o do movimento

    private float fixedY; // Posi��o fixa no eixo Y
    private float fixedZ; // Posi��o fixa no eixo Z

    private void Start()
    {
        // Salva as posi��es Y e Z iniciais da c�mera
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player n�o foi atribu�do � c�mera.");
            return;
        }

        // Calcula a nova posi��o no eixo X
        Vector3 targetPosition = new Vector3(player.position.x + offsetX, fixedY, fixedZ);

        // Suaviza a transi��o entre a posi��o atual e a posi��o-alvo
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Atualiza a posi��o da c�mera
        transform.position = smoothedPosition;
    }
}
