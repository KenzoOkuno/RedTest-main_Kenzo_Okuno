using UnityEngine;

public class HideMouseCursor : MonoBehaviour
{
    private void Start() => Hide();

    private void Hide()
    {
        // Escondendo cursor
        Cursor.visible = false;

        // Travando sua posi��o
        Cursor.lockState = CursorLockMode.Locked;
    }
}