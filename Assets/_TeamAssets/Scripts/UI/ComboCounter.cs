using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboCounter : MonoBehaviour
{
    [Header("UI Reference")]
    public Text comboText; // Referência ao texto do combo

    [Header("Combo Settings")]
    public float comboResetTime = 1.5f; // Tempo limite para resetar o combo
    public float textAnimationSizeIncrease = 2f; // Tamanho extra na animação
    public float textAnimationDuration = 0.2f; // Duração da animação

    private int comboCount = 0;
    private Coroutine resetCoroutine;
    private int originalFontSize;

    private void Start()
    {
        if (comboText != null)
        {
            originalFontSize = comboText.fontSize;
            UpdateComboText();
        }
        else
        {
            Debug.LogError("ComboCounter: comboText não foi atribuído!");
        }
    }

    public void IncreaseCombo()
    {
        comboCount++;
        UpdateComboText();

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetComboAfterDelay());
    }

    private IEnumerator ResetComboAfterDelay()
    {
        yield return new WaitForSeconds(comboResetTime);
        comboCount = 0;
        UpdateComboText();
    }

    private void UpdateComboText()
    {
        comboText.text = $"COMBO : X{comboCount}";
        StartCoroutine(AnimateTextSize());
    }

    private IEnumerator AnimateTextSize()
    {
        comboText.fontSize = Mathf.RoundToInt(originalFontSize + textAnimationSizeIncrease);
        yield return new WaitForSeconds(textAnimationDuration);
        comboText.fontSize = originalFontSize;
    }
}
