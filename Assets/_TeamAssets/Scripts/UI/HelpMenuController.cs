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
            

            // Procurando o Action Map "UI" e a a��o "HelpMenu"
            var uiActionMap = playerInput.actions.FindActionMap("UI");

            if (uiActionMap != null)
            {
                
                helpAction = uiActionMap.FindAction("HelpMenu");

                
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
