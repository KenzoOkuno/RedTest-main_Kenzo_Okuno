using UnityEngine;
using UnityEngine.InputSystem;

public class HelpMenuController : MonoBehaviour
{
    public GameObject helpMenu;  // Arraste a imagem do menu no Inspector
    private PlayerInput playerInput;
    private InputAction helpAction;
    private bool isOpen = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("[HelpMenuController] ERRO: PlayerInput não encontrado!");
            return;
        }

        helpAction = playerInput.actions["HelpMenu"]; // Obtém a ação diretamente

        if (helpAction == null)
        {
            Debug.LogError("[HelpMenuController] ERRO: Ação 'HelpMenu' NÃO encontrada!");
        }

        // Garante que o menu comece fechado
        if (helpMenu != null)
        {
            helpMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("[HelpMenuController] ERRO: Nenhum GameObject atribuído ao helpMenu!");
        }
    }

    private void OnEnable()
    {
        if (helpAction != null)
        {
            helpAction.performed += ToggleHelpMenu;
        }
    }

    private void OnDisable()
    {
        if (helpAction != null)
        {
            helpAction.performed -= ToggleHelpMenu;
        }
    }

    private void ToggleHelpMenu(InputAction.CallbackContext context)
    {
        if (helpMenu == null) return;

        isOpen = !isOpen;
        helpMenu.SetActive(isOpen);
        Debug.Log($"[HelpMenuController] Menu de ajuda {(isOpen ? "ABERTO" : "FECHADO")}.");
    }
}
