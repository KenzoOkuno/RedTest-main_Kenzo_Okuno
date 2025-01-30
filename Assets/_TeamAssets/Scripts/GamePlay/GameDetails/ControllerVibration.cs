using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ControllerVibration : MonoBehaviour
{
    public static ControllerVibration Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Vibrate(float duration, float intensity)
    {
        if (Gamepad.current != null)
        {
            StartCoroutine(VibrationCoroutine(duration, intensity));
        }
    }

    private IEnumerator VibrationCoroutine(float duration, float intensity)
    {
        Gamepad.current.SetMotorSpeeds(intensity, intensity);
        yield return new WaitForSeconds(duration);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
}