using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEnergyController : MonoBehaviour
{
    #region UI References
    [Header("UI References")]
    public Slider energyBar; // Barra de energia (UI Slider)
    #endregion

    #region Energy Settings
    [Header("Energy Settings")]
    [SerializeField] private float energyRechargeTime = 5f; // Tempo total para encher a barra
    private float currentEnergy = 0f; // Valor atual da barra de energia

    private bool canUseSpecial = false; // Controle para verificar se o ataque especial est� dispon�vel
    private Coroutine rechargeCoroutine; // Refer�ncia � corrotina de recarga
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Inicializa a barra de energia
        if (energyBar != null)
        {
            energyBar.value = 0f;
            energyBar.maxValue = energyRechargeTime;
        }

        // Inicia a recarga da energia
        StartEnergyRecharge();
    }

    private void Update()
    {
        // Este m�todo pode ser �til se voc� quiser checar alguma condi��o constantemente, mas neste caso n�o h� l�gica de atualiza��o direta.
    }
    #endregion

    #region Energy Recharge Logic
    private void StartEnergyRecharge()
    {
        // Garantir que n�o h� corrotinas anteriores rodando
        if (rechargeCoroutine != null)
            StopCoroutine(rechargeCoroutine);

        rechargeCoroutine = StartCoroutine(RechargeEnergy());
    }

    private IEnumerator RechargeEnergy()
    {
        // Recarrega a energia durante o tempo, mas sem interferir na movimenta��o
        while (currentEnergy < energyRechargeTime)
        {
            currentEnergy += Time.deltaTime; // Aumenta a energia conforme o tempo passa
            if (energyBar != null)
                energyBar.value = currentEnergy; // Atualiza o valor da barra de energia

            yield return null; // Aguarda o pr�ximo frame
        }

        currentEnergy = energyRechargeTime; // Garantir que a energia n�o passe do m�ximo
        canUseSpecial = true; // Ativa o ataque especial
    }
    #endregion

    #region Energy Reset Logic
    private void ResetEnergyBar()
    {
        // Reseta a barra de energia
        currentEnergy = 0f;
        if (energyBar != null)
            energyBar.value = currentEnergy;

        canUseSpecial = false; // Bloqueia o ataque especial at� a recarga
        StartEnergyRecharge(); // Reinicia a recarga da barra
    }
    #endregion

    #region Special Attack Logic
    // M�todo p�blico para ativar o ataque especial externamente
    public void TriggerEnergySpecialAttack()
    {
        if (canUseSpecial)
        {
            ResetEnergyBar(); // Reseta a barra de energia quando o ataque especial � ativado
        }
    }
    #endregion

    #region Energy Modification Logic
    // M�todo de controle de energia, para caso outro script precise modificar a energia diretamente
    public void ModifyEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, energyRechargeTime); // Evita que a energia ultrapasse os limites
        if (energyBar != null)
            energyBar.value = currentEnergy; // Atualiza a barra de energia

        // Checa se a energia atingiu o m�ximo
        canUseSpecial = currentEnergy >= energyRechargeTime;
    }
    #endregion
}
