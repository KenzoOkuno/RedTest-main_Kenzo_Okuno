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

    private bool canUseSpecial = false; // Controle para verificar se o ataque especial est� dispon�vel
    private Coroutine rechargeCoroutine; // Refer�ncia � corrotina de recarga

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
            currentEnergy += Time.deltaTime;
            if (energyBar != null)
                energyBar.value = currentEnergy;

            yield return null; // Aguarda o pr�ximo frame
        }

        currentEnergy = energyRechargeTime; // Garantir que a energia n�o passe do m�ximo
        canUseSpecial = true; // Ativa o ataque especial
    }

    private void ResetEnergyBar()
    {
        // Reseta a barra de energia
        currentEnergy = 0f;
        if (energyBar != null)
            energyBar.value = currentEnergy;

        canUseSpecial = false; // Bloqueia o ataque especial at� a recarga
        StartEnergyRecharge(); // Reinicia a recarga da barra
    }

    // M�todo p�blico para ativar o ataque especial externamente
    public void TriggerEnergySpecialAttack()
    {
        if (canUseSpecial)
        {
            ResetEnergyBar(); // Reseta a barra de energia
        }
    }

    // M�todo de controle de energia, para caso outro script precise modificar a energia diretamente
    public void ModifyEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, energyRechargeTime);
        if (energyBar != null)
            energyBar.value = currentEnergy;

        // Checa se a energia atingiu o m�ximo
        canUseSpecial = currentEnergy >= energyRechargeTime;
    }
}
