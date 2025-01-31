using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ControllerVibration : MonoBehaviour
{
    #region Singleton
    public static ControllerVibration Instance; // Instância singleton para acesso global
    #endregion

    #region Initialization
    private void Awake()
    {
        // Inicializa a instância singleton
        Instance = this;
    }
    #endregion

    #region Vibration Logic
    public void Vibrate(float duration, float intensity)
    {
        // Verifica se um Gamepad está conectado e inicia a vibração
        if (Gamepad.current != null)
        {
            StartCoroutine(VibrationCoroutine(duration, intensity));
        }
    }

    private IEnumerator VibrationCoroutine(float duration, float intensity)
    {
        // Configura as vibrações do Gamepad
        Gamepad.current.SetMotorSpeeds(intensity, intensity);

        // Espera a duração especificada para a vibração
        yield return new WaitForSeconds(duration);

        // Desativa as vibrações após o tempo
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
    #endregion
}
