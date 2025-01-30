using UnityEngine;

public class PlayerComboController : MonoBehaviour
{
    #region Vari�veis Privadas
    private int comboStep = 0; // Etapa atual do combo
    private float comboTimer = 0f; // Tempo restante para a execu��o do pr�ximo golpe
    #endregion

    #region Vari�veis P�blicas
    public float comboTimeWindow = 1f; // Janela de tempo permitida para continuar o combo
    #endregion

    #region M�todos de Controle do Combo
    /// <summary>
    /// Inicia o combo definindo o primeiro ataque e resetando o timer.
    /// </summary>
    public void StartCombo()
    {
        comboStep = 1; // Inicia o combo com o primeiro golpe
        comboTimer = comboTimeWindow; // Reseta o tempo dispon�vel para continuar o combo
    }

    /// <summary>
    /// Continua o combo se ainda estiver dentro da janela de tempo.
    /// </summary>
    public void ContinueCombo()
    {
        if (comboTimer > 0f)
        {
            comboStep++; // Avan�a para o pr�ximo ataque no combo
            comboTimer = comboTimeWindow; // Reseta o tempo dispon�vel para o pr�ximo ataque
        }
    }

    /// <summary>
    /// Atualiza o timer do combo e reseta caso o tempo tenha expirado.
    /// </summary>
    public void UpdateCombo()
    {
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            comboStep = 0; // Reseta o combo se o tempo acabar
        }
    }
    #endregion

    #region M�todos de Consulta
    /// <summary>
    /// Retorna a etapa atual do combo.
    /// </summary>
    public int GetComboStep()
    {
        return comboStep;
    }

    /// <summary>
    /// Retorna verdadeiro se um combo estiver ativo (ou seja, n�o foi resetado).
    /// </summary>
    public bool IsComboActive()
    {
        return comboStep > 0;
    }
    #endregion
}
