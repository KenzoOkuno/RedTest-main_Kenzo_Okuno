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
            Debug.LogError("[HelpMenuController] ERRO: PlayerInput n�o encontrado!");
            return;
        }

        helpAction = playerInput.actions["HelpMenu"]; // Obt�m a a��o diretamente

        if (helpAction == null)
        {
            Debug.LogError("[HelpMenuController] ERRO: A��o 'HelpMenu' N�O encontrada!");
        }

        // Garante que o menu comece fechado
        if (helpMenu != null)
        {
            helpMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("[HelpMenuController] ERRO: Nenhum GameObject atribu�do ao helpMenu!");
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
