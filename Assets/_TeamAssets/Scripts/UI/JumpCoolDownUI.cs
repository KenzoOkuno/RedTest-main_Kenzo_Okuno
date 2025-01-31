using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class JumpCooldownUI : MonoBehaviour
{
    public TextMeshProUGUI jumpCooldownText;  // Arraste o TextMeshPro da UI no Inspector
    private MovingState movingState;

    private void Start()
    {
        // Busca o MovingState automaticamente no Player
        movingState = FindFirstObjectByType<MovingState>();


        // Inicia o texto como "JUMP READY"
        UpdateCooldownText(0);
    }

    public void StartCooldownUI(float cooldownTime)
    {
        StartCoroutine(UpdateCooldownUI(cooldownTime));
    }

    private IEnumerator UpdateCooldownUI(float cooldownTime)
    {
        float remainingTime = cooldownTime;

        while (remainingTime > 0)
        {
            jumpCooldownText.text = $"Jump Cooldown: {remainingTime:F1}s";
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        // Quando acabar o cooldown, exibir "JUMP READY"
        UpdateCooldownText(0);
    }

    private void UpdateCooldownText(float time)
    {
        if (time > 0)
        {
            jumpCooldownText.text = $"Jump Cooldown: {time:F1}s";
        }
        else
        {
            jumpCooldownText.text = "JUMP READY";
        }
    }
}
