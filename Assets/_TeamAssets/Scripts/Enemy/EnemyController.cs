using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    #region Variables
    public float health = 100f; // Vida do inimigo
    public float maxHealth = 200f; // Vida máxima do inimigo
    public float knockbackForce = 5f; // Força do knockback

    private EnemyAnimationController enemyAnimationController;
    public GameObject damageEffect; // Referência ao objeto de efeito de dano (GIF)
    public Transform damageEffectSpawnPoint; // Ponto de spawn do efeito (pode ser um Transform no personagem)
    private Rigidbody rb; // Rigidbody 3D do inimigo
    public AudioClip damageSound; // Referência ao clip de áudio de dano
    public AudioClip deathSound; // Referência ao clip de áudio de morte
    private AudioSource audioSource; // Componente AudioSource

    [Header("Health Bar Setup")]
    public GameObject healthBarPrefab; // Prefab da barra de vida
    public Image healthBarFill; // Preenchimento da barra de vida (referência pública)
    private GameObject instantiatedHealthBar; // Instância da barra de vida

    private bool isDead = false; // Verifica se o inimigo está morto
    #endregion

    #region Initialization
    private void Awake()
    {
        // Inicializa os componentes necessários
        rb = GetComponent<Rigidbody>();
        enemyAnimationController = GetComponent<EnemyAnimationController>();
        audioSource = GetComponent<AudioSource>();

       

        // Instancia a barra de vida no ponto de spawn
        if (healthBarPrefab != null)
        {
            

            // Configuração do healthBarFill caso não tenha sido configurado no Inspector
            if (healthBarFill == null)
            {
                healthBarFill = instantiatedHealthBar.GetComponentInChildren<Image>();
                if (healthBarFill == null)
                {
                    Debug.LogError("Erro: O 'healthBarFill' não foi encontrado no prefab!");
                }
            }
        }
        else
        {
            Debug.LogError("Erro: healthBarPrefab ou healthBarSpawnPoint não estão configurados no Inspector!");
        }
    }
    #endregion

    #region Health Bar Management
    private void Update()
    {
        // Opcional: Faz a barra de vida sempre encarar a câmera
        if (instantiatedHealthBar != null)
        {
            instantiatedHealthBar.transform.LookAt(Camera.main.transform);
        }
    }
    #endregion

    #region Damage Handling
    private void OnTriggerEnter(Collider other)
    {
        // Detecta ataques do jogador
        

        if (other.CompareTag("PlayerAttack"))
        {
            
            TakeDamage(20f); // Aplica dano (valor de exemplo)
            ApplyKnockback(other); // Aplica knockback baseado na direção do atacante
            ActivateDamageEffect();
            PlayDamageSound();
            CameraShake.Instance.Shake(0.3f, 0.2f);
            ControllerVibration.Instance.Vibrate(0.5f, 0.7f);
        }

        if (other.CompareTag("PlayerSpecialAttack"))
        {
           
            TakeDamage(100f); // Aplica dano (valor de exemplo)
            ApplyKnockback(other); // Aplica knockback baseado na direção do atacante
            ActivateDamageEffect();
            PlayDamageSound();
            CameraShake.Instance.Shake(0.3f, 0.2f);
            ControllerVibration.Instance.Vibrate(0.5f, 0.7f);
        }
    }

    public void PlayDamageSound()
    {
        // Toca o som de dano
        if (audioSource != null && damageSound != null)
        {
            
            audioSource.PlayOneShot(damageSound);
        }
        else
        {
            Debug.LogError("Erro: AudioSource ou DamageSound não configurados.");
        }
    }

    public void PlayDeathSound()
    {
        // Toca o som de morte
        if (audioSource != null && deathSound != null)
        {
          
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogError("Erro: AudioSource ou DamageSound não configurados.");
        }
    }

    private void TakeDamage(float damage)
    {
        // Aplica dano ao inimigo
        if (isDead) return; // Não aplica mais dano se o inimigo estiver morto

       

        health -= damage;
       

        ComboCounter comboCounter = FindFirstObjectByType<ComboCounter>();
        if (comboCounter != null)
        {
            comboCounter.IncreaseCombo();
        }

        // Atualiza a barra de vida
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Clamp01(health / maxHealth);
            
        }
        else
        {
            Debug.LogError("Erro: healthBarFill está nulo ao tentar atualizar.");
        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            PlayDeathSound();
            StartCoroutine(HandleDeath()); // Inicia o processo de morte
        }
        else
        {
           
            enemyAnimationController.TakeDamage();
        }
    }
    #endregion

    #region Death Handling
    private System.Collections.IEnumerator HandleDeath()
    {
        // Aguardar 4 segundos antes de destruir
        enemyAnimationController.Die();
        yield return new WaitForSeconds(4f);

        // Destruir o inimigo após a animação
        Destroy(gameObject);
    }
    #endregion

    #region Knockback Handling
    private void ApplyKnockback(Collider attackCollider)
    {
        // Aplica o efeito de knockback
        

        if (rb != null)
        {
            Transform attackerTransform = attackCollider.transform.root;
            Vector3 knockbackDirection = (transform.position - attackerTransform.position).normalized;
            knockbackDirection.y = 0; // Mantém no plano horizontal
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            StartCoroutine(StopKnockback());
           
        }
        else
        {
            Debug.LogError("Erro: Rigidbody do inimigo está nulo.");
        }
    }

    private System.Collections.IEnumerator StopKnockback()
    {
        // Para o knockback após um curto período
        yield return new WaitForSeconds(0.2f);
        rb.freezeRotation = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
       
    }
    #endregion

    #region Damage Effect Handling
    private void ActivateDamageEffect()
    {
        // Ativa o efeito de dano
        
        if (damageEffect != null)
        {
            damageEffect.SetActive(true);
            damageEffect.transform.position = damageEffectSpawnPoint.position;
            Invoke("DeactivateDamageEffect", 1f);
            
        }
        else
        {
            Debug.LogError("Erro: damageEffect não está configurado.");
        }
    }

    private void DeactivateDamageEffect()
    {
        // Desativa o efeito de dano
       
        if (damageEffect != null)
        {
            damageEffect.SetActive(false);
        }
    }
    #endregion
}
