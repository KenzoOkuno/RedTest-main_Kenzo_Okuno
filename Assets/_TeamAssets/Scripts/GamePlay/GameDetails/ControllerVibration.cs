using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ControllerVibration : MonoBehaviour
{
    #region Singleton
    public static ControllerVibration Instance; // Inst�ncia singleton para acesso global
    #endregion

    #region Initialization
    private void Awake()
    {
        // Inicializa a inst�ncia singleton
        Instance = this;
    }
    #endregion

    #region Vibration Logic
    public void Vibrate(float duration, float intensity)
    {
        // Verifica se um Gamepad est� conectado e inicia a vibra��o
        if (Gamepad.current != null)
        {
            StartCoroutine(VibrationCoroutine(duration, intensity));
        }
    }

    private IEnumerator VibrationCoroutine(float duration, float intensity)
    {
        // Configura as vibra��es do Gamepad
        Gamepad.current.SetMotorSpeeds(intensity, intensity);

        // Espera a dura��o especificada para a vibra��o
        yield return new WaitForSeconds(duration);

        // Desativa as vibra��es ap�s o tempo
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
    #endregion
}
