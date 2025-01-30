using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEnergyController : MonoBehaviour
{
    [Header("UI References")]
    public Slider energyBar; // Barra de energia (UI Slider)

    [Header("Energy Settings")]
    [SerializeField] private float energyRechargeTime = 5f; // Tempo total para encher a barra
    private float currentEnergy = 0f; // Valor atual da barra de energia

    private bool canUseSpecial = false; // Controle para verificar se o ataque especial está disponível
    private Coroutine rechargeCoroutine; // Referência à corrotina de recarga

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

    private void StartEnergyRecharge()
    {
        // Garantir que não há corrotinas anteriores rodando
        if (rechargeCoroutine != null)
            StopCoroutine(rechargeCoroutine);

        rechargeCoroutine = StartCoroutine(RechargeEnergy());
    }

    private IEnumerator RechargeEnergy()
    {
        // Recarrega a energia durante o tempo, mas sem interferir na movimentação
        while (currentEnergy < energyRechargeTime)
        {
            currentEnergy += Time.deltaTime;
            if (energyBar != null)
                energyBar.value = currentEnergy;

            yield return null; // Aguarda o próximo frame
        }

        currentEnergy = energyRechargeTime; // Garantir que a energia não passe do máximo
        canUseSpecial = true; // Ativa o ataque especial
    }

    private void ResetEnergyBar()
    {
        // Reseta a barra de energia
        currentEnergy = 0f;
        if (energyBar != null)
            energyBar.value = currentEnergy;

        canUseSpecial = false; // Bloqueia o ataque especial até a recarga
        StartEnergyRecharge(); // Reinicia a recarga da barra
    }

    // Método público para ativar o ataque especial externamente
    public void TriggerEnergySpecialAttack()
    {
        if (canUseSpecial)
        {
            ResetEnergyBar(); // Reseta a barra de energia
        }
    }

    // Método de controle de energia, para caso outro script precise modificar a energia diretamente
    public void ModifyEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, energyRechargeTime);
        if (energyBar != null)
            energyBar.value = currentEnergy;

        // Checa se a energia atingiu o máximo
        canUseSpecial = currentEnergy >= energyRechargeTime;
    }
}
