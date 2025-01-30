using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    #region Variáveis
    public float health = 100f;
    public float maxHealth = 200f;
    public float knockbackForce = 5f;

    private EnemyAnimationController enemyAnimationController;
    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isDead = false;

    [Header("Referências Visuais")]
    public GameObject damageEffect;
    public Transform damageEffectSpawnPoint;

    [Header("Configuração de Áudio")]
    public AudioClip damageSound;
    public AudioClip deathSound;

    [Header("Configuração da Barra de Vida")]
    public GameObject healthBarPrefab;
    public Image healthBarFill;
    private GameObject instantiatedHealthBar;
    #endregion

    #region Métodos Unity
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyAnimationController = GetComponent<EnemyAnimationController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (instantiatedHealthBar != null)
        {
            instantiatedHealthBar.transform.LookAt(Camera.main.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            TakeDamage(20f);
            ApplyKnockback(other);
            ActivateDamageEffect();
            PlayDamageSound();
            CameraShake.Instance.Shake(0.3f, 0.2f);
            ControllerVibration.Instance.Vibrate(0.5f, 0.7f);
        }
        else if (other.CompareTag("PlayerSpecialAttack"))
        {
            TakeDamage(100f);
            ApplyKnockback(other);
            ActivateDamageEffect();
            PlayDamageSound();
            CameraShake.Instance.Shake(0.3f, 0.2f);
            ControllerVibration.Instance.Vibrate(0.5f, 0.7f);
        }
    }
    #endregion

    #region Métodos de Dano e Morte
    private void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        ComboCounter comboCounter = FindFirstObjectByType<ComboCounter>();
        comboCounter?.IncreaseCombo();

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Clamp01(health / maxHealth);
        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            PlayDeathSound();
            StartCoroutine(HandleDeath());
        }
        else
        {
            enemyAnimationController.TakeDamage();
        }
    }

    private IEnumerator HandleDeath()
    {
        enemyAnimationController.Die();
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
    #endregion

    #region Métodos de Efeitos e Feedback
    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    private void ActivateDamageEffect()
    {
        if (damageEffect != null)
        {
            damageEffect.SetActive(true);
            damageEffect.transform.position = damageEffectSpawnPoint.position;
            Invoke("DeactivateDamageEffect", 1f);
        }
    }

    private void DeactivateDamageEffect()
    {
        if (damageEffect != null)
        {
            damageEffect.SetActive(false);
        }
    }
    #endregion

    #region Métodos de Knockback
    private void ApplyKnockback(Collider attackCollider)
    {
        if (rb != null)
        {
            Transform attackerTransform = attackCollider.transform.root;
            Vector3 knockbackDirection = (transform.position - attackerTransform.position).normalized;
            knockbackDirection.y = 0;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            StartCoroutine(StopKnockback());
        }
    }

    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        rb.freezeRotation = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    #endregion
}
