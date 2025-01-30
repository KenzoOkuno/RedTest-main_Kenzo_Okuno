using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboCounter : MonoBehaviour
{
    public Text comboText;  // Referência ao texto do combo
    private int comboCount = 0;
    private float comboResetTime = 1.5f; // Tempo limite para resetar o combo
    private Coroutine resetCoroutine;    // Para armazenar a coroutine de reset

    private void Start()
    {
        UpdateComboText();
    }

    public void IncreaseCombo()
    {
        comboCount++;
        UpdateComboText();
        

        // Reseta o contador de tempo
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
        AnimateText();
    }

    private void AnimateText()
    {
        StartCoroutine(TextSizeAnimation());
    }

    private IEnumerator TextSizeAnimation()
    {
        float originalSize = comboText.fontSize;
        comboText.fontSize = Mathf.RoundToInt(originalSize + 2f); // Aumenta temporariamente o tamanho
        yield return new WaitForSeconds(0.2f); // Pequeno delay para a animação
        comboText.fontSize = Mathf.RoundToInt(originalSize); // Retorna ao tamanho original
    }
}
