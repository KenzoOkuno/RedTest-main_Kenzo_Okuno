using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class JumpCooldownUI : MonoBehaviour
{
    public TextMeshProUGUI jumpCooldownText;
    private MovingState movingState;
    private Coroutine cooldownCoroutine; // Para garantir que apenas uma corrotina esteja rodando

    private void Start()
    {
        movingState = FindFirstObjectByType<MovingState>();

        if (movingState == null)
        {
            Debug.LogError("[JumpCooldownUI] ERRO: MovingState não encontrado!");
        }
        else
        {
            Debug.Log("[JumpCooldownUI] MovingState encontrado!");
        }

        UpdateCooldownUI(0);
    }

    public void StartCooldownUI(float cooldownTime)
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
        cooldownCoroutine = StartCoroutine(UpdateCooldownUI(cooldownTime));
    }

    private IEnumerator UpdateCooldownUI(float cooldownTime)
    {
        float remainingTime = cooldownTime;

        while (remainingTime > 0)
        {
            jumpCooldownText.text = $"Jump Cooldown: {remainingTime:F1}s";
            yield return null;
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0) remainingTime = 0; // Evita valores negativos
        }

        jumpCooldownText.text = "JUMP READY";
    }
}
