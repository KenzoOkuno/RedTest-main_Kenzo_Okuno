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

        if (playerInput != null)
        {
            Debug.Log("[HelpMenuController] PlayerInput encontrado.");

            // Procurando o Action Map "UI" e a a��o "HelpMenu"
            var uiActionMap = playerInput.actions.FindActionMap("UI");

            if (uiActionMap != null)
            {
                Debug.Log("[HelpMenuController] Action Map 'UI' encontrado.");
                helpAction = uiActionMap.FindAction("HelpMenu");

                if (helpAction != null)
                {
                    Debug.Log("[HelpMenuController] A��o 'HelpMenu' encontrada!");
                }
                else
                {
                    Debug.LogError("[HelpMenuController] ERRO: A��o 'HelpMenu' N�O encontrada no Action Map 'UI'!");
                }
            }
            else
            {
                Debug.LogError("[HelpMenuController] ERRO: Action Map 'UI' N�O encontrado!");
            }
        }
        else
        {
            Debug.LogError("[HelpMenuController] ERRO: PlayerInput n�o encontrado no GameObject!");
        }
    }

    private void OnEnable()
    {
        if (helpAction != null)
        {
            helpAction.performed += ToggleHelpMenu;
            Debug.Log("[HelpMenuController] Evento de entrada registrado para 'HelpMenu'.");
        }
    }

    private void OnDisable()
    {
        if (helpAction != null)
        {
            helpAction.performed -= ToggleHelpMenu;
            Debug.Log("[HelpMenuController] Evento de entrada removido para 'HelpMenu'.");
        }
    }

    private void ToggleHelpMenu(InputAction.CallbackContext context)
    {
        isOpen = !isOpen;
        helpMenu.SetActive(isOpen);
        Debug.Log("[HelpMenuController] Menu de ajuda " + (isOpen ? "ABERTO" : "FECHADO") + ".");
    }
}
