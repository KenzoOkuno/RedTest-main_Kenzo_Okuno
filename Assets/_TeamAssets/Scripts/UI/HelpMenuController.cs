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
            

            // Procurando o Action Map "UI" e a ação "HelpMenu"
            var uiActionMap = playerInput.actions.FindActionMap("UI");

            if (uiActionMap != null)
            {
                
                helpAction = uiActionMap.FindAction("HelpMenu");

                
            }
            else
            {
                Debug.LogError("[HelpMenuController] ERRO: Action Map 'UI' NÃO encontrado!");
            }
        }
        else
        {
            Debug.LogError("[HelpMenuController] ERRO: PlayerInput não encontrado no GameObject!");
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
        isOpen = !isOpen;
        helpMenu.SetActive(isOpen);
        
    }
}
