using UnityEngine;

public class PlayerComboController : MonoBehaviour
{
    #region Variáveis Privadas
    private int comboStep = 0; // Etapa atual do combo
    private float comboTimer = 0f; // Tempo restante para a execução do próximo golpe
    #endregion

    #region Variáveis Públicas
    public float comboTimeWindow = 1f; // Janela de tempo permitida para continuar o combo
    #endregion

    #region Métodos de Controle do Combo
    /// <summary>
    /// Inicia o combo definindo o primeiro ataque e resetando o timer.
    /// </summary>
    public void StartCombo()
    {
        comboStep = 1; // Inicia o combo com o primeiro golpe
        comboTimer = comboTimeWindow; // Reseta o tempo disponível para continuar o combo
    }

    /// <summary>
    /// Continua o combo se ainda estiver dentro da janela de tempo.
    /// </summary>
    public void ContinueCombo()
    {
        if (comboTimer > 0f)
        {
            comboStep++; // Avança para o próximo ataque no combo
            comboTimer = comboTimeWindow; // Reseta o tempo disponível para o próximo ataque
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

    #region Métodos de Consulta
    /// <summary>
    /// Retorna a etapa atual do combo.
    /// </summary>
    public int GetComboStep()
    {
        return comboStep;
    }

    /// <summary>
    /// Retorna verdadeiro se um combo estiver ativo (ou seja, não foi resetado).
    /// </summary>
    public bool IsComboActive()
    {
        return comboStep > 0;
    }
    #endregion
}
