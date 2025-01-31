using UnityEngine;

public class PlayerComboController : MonoBehaviour
{
    #region Variables
    private int comboStep = 0; // Etapa do combo atual
    private float comboTimer = 0f; // Tempo restante para o combo
    public float comboTimeWindow = 1f; // Janela de tempo para continuar o combo
    #endregion

    #region Combo Control
    public void StartCombo()
    {
        comboStep = 1; // Inicia o combo com o primeiro golpe
        comboTimer = comboTimeWindow; // Reseta o tempo do combo
    }

    public void ContinueCombo()
    {
        if (comboTimer > 0f)
        {
            comboStep++; // Avança para o próximo golpe do combo
            comboTimer = comboTimeWindow; // Reseta o timer para continuar o combo
        }
    }
    #endregion

    #region Combo Update
    public void UpdateCombo()
    {
        // Atualiza o tempo do combo, e se exceder o limite, reseta o combo
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            comboStep = 0; // Reseta o combo
        }
    }
    #endregion

    #region Combo State
    public int GetComboStep()
    {
        return comboStep;
    }

    public bool IsComboActive()
    {
        return comboStep > 0;
    }
    #endregion
}
