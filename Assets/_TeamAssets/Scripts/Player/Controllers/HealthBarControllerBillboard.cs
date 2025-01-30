using UnityEngine;

public class HealthBarControllerBillboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // Obtém a câmera principal
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // Faz a barra de vida olhar diretamente para a câmera
        transform.forward = mainCamera.transform.forward;
    }
}
