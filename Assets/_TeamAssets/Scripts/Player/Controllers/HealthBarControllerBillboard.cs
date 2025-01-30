using UnityEngine;

public class HealthBarControllerBillboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // Obt�m a c�mera principal
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // Faz a barra de vida olhar diretamente para a c�mera
        transform.forward = mainCamera.transform.forward;
    }
}
