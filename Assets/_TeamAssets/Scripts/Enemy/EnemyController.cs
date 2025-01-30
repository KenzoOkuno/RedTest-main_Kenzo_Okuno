using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float health = 100f; // Vida do inimigo
    public float maxHealth = 200f; // Vida máxima do inimigo
    public float knockbackForce = 5f; // Força do knockback

    private EnemyAnimationController enemyAnimationController;
    public GameObject damageEffect; // Referência ao objeto de efeito de dano (GIF)
    public Transform damageEffectSpawnPoint; // Ponto de spawn do efeito (pode ser um Transform no personagem)
    private Rigidbody rb; // Rigidbody 3D do inimigo
    public AudioClip damageSound; // Referência ao clip de áudio de dano
    public AudioClip deathSound; // Referência ao clip de áudio de dano
    private AudioSource audioSource; // Componente AudioSource

    [Header("Health Bar Setup")]
    public GameObject healthBarPrefab; // Prefab da barra de vida
    public Image healthBarFill; // Preenchimento da barra de vida (referência pública)
    private GameObject instantiatedHealthBar; // Instância da barra de vida

    private bool isDead = false; // Verifica se o inimigo está morto

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyAnimationController = GetComponent<EnemyAnimationController>();
        audioSource = GetComponent<AudioSource>();

        Debug.Log("Awake chamado.");

        // Instancia a barra de vida no ponto de spawn
        if (healthBarPrefab != null)
        {
        //    instantiatedHealthBar = Instantiate(healthBarPrefab, healthBarSpawnPoint.position, Quaternion.identity);
            Debug.Log("Barra de vida instanciada.");

            // Caso healthBarFill não tenha sido configurada no Inspector
            if (healthBarFill == null)
            {
                healthBarFill = instantiatedHealthBar.GetComponentInChildren<Image>();
                if (healthBarFill == null)
                {
                    Debug.LogError("Erro: O 'healthBarFill' não foi encontrado no prefab!");
                }
                else
                {
                    Debug.Log("HealthBarFill configurado com sucesso.");
                }
            }

            // Faz a barra de vida seguir o inimigo
           // instantiatedHealthBar.transform.SetParent(healthBarSpawnPoint, true);
        }
        else
        {
            Debug.LogError("Erro: healthBarPrefab ou healthBarSpawnPoint não estão configurados no Inspector!");
        }
    }

    private void Update()
    {
        // Opcional: Faz a barra de vida sempre encarar a câmera
        if (instantiatedHealthBar != null)
        {
            instantiatedHealthBar.transform.LookAt(Camera.main.transform);
        }
    }

    // Método chamado quando algo entra no collider do inimigo
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter chamado com o objeto: {other.gameObject.name}");

        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("PlayerAttack detectado.");
            TakeDamage(20f); // Aplica dano (valor de exemplo)
            ApplyKnockback(other); // Aplica knockback baseado na direção do atacante
            ActivateDamageEffect();
            PlayDamageSound();
            CameraShake.Instance.Shake(0.3f, 0.2f);
            ControllerVibration.Instance.Vibrate(0.5f, 0.7f);


        }

        if (other.CompareTag("PlayerSpecialAttack"))
        {
            Debug.Log("PlayerSpecialAttack detectado.");
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
        if (audioSource != null && damageSound != null)
        {
            Debug.Log("Tocando som de dano.");
            audioSource.PlayOneShot(damageSound);
        }
        else
        {
            Debug.LogError("Erro: AudioSource ou DamageSound não configurados.");
        }
    }

    public void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            Debug.Log("Tocando som de dano.");
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogError("Erro: AudioSource ou DamageSound não configurados.");
        }
    }

    private void TakeDamage(float damage)
    {
        if (isDead) return; // Não aplica mais dano se o inimigo estiver morto

        Debug.Log($"TakeDamage chamado com dano: {damage}");

        health -= damage;
        Debug.Log($"Nova vida do inimigo: {health}");

        // Atualiza a barra de vida
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Clamp01(health / maxHealth);
            Debug.Log($"Barra de vida atualizada: {healthBarFill.fillAmount}");
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
            Debug.Log("Inimigo recebeu dano, mas ainda está vivo.");
            enemyAnimationController.TakeDamage();
        }
    }

    private System.Collections.IEnumerator HandleDeath()
    {
        // Aguardar 4 segundos antes de destruir
        enemyAnimationController.Die();
        yield return new WaitForSeconds(4f);

        // Ativar a animação de morte
        

        // Destruir o inimigo após a animação
        Destroy(gameObject);
    }

    private void ApplyKnockback(Collider attackCollider)
    {
        Debug.Log("ApplyKnockback chamado.");

        if (rb != null)
        {
            Transform attackerTransform = attackCollider.transform.root;
            Vector3 knockbackDirection = (transform.position - attackerTransform.position).normalized;
            knockbackDirection.y = 0; // Mantém no plano horizontal
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            StartCoroutine(StopKnockback());
            Debug.Log("Knockback aplicado.");
        }
        else
        {
            Debug.LogError("Erro: Rigidbody do inimigo está nulo.");
        }
    }

    private System.Collections.IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("Knockback parado.");
    }

    private void ActivateDamageEffect()
    {
        Debug.Log("ActivateDamageEffect chamado.");
        if (damageEffect != null)
        {
            damageEffect.SetActive(true);
            damageEffect.transform.position = damageEffectSpawnPoint.position;
            Invoke("DeactivateDamageEffect", 1f);
            Debug.Log("Efeito de dano ativado.");
        }
        else
        {
            Debug.LogError("Erro: damageEffect não está configurado.");
        }
    }

    private void DeactivateDamageEffect()
    {
        Debug.Log("DeactivateDamageEffect chamado.");
        if (damageEffect != null)
        {
            damageEffect.SetActive(false);
        }
    }
}