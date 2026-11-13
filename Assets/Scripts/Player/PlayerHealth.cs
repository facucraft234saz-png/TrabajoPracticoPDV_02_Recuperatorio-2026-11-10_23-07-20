
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Inmunidad al daño")]
    [SerializeField] private float damageCooldown = 1.5f;

    private float nextDamageTime;
    private bool isDead;

    // Datos publicos para otros sistemas
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;
    public bool IsInvulnerable => Time.time < nextDamageTime;

    // Eventos para otros sistemas
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );
    }

    private void Start()
    {
        Debug.Log(
            "Salud inicial: " +
            currentHealth + "/" + maxHealth
        );

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0f)
            return;

        // Comprobar inmunidad temporal
        if (Time.time < nextDamageTime)
        {
            Debug.Log("Jugador protegido por inmunidad.");
            return;
        }

        // Activar el cooldown
        nextDamageTime = Time.time + damageCooldown;

        currentHealth = Mathf.Max(
            0f,
            currentHealth - damage
        );

        Debug.Log("Jugador recibio " + damage + " de daño.");

        NotifyHealthChanged();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        float previousHealth = currentHealth;

        currentHealth = Mathf.Min(
            maxHealth,
            currentHealth + amount
        );

        if (currentHealth > previousHealth)
        {
            Debug.Log(
                "Jugador recupero " +
                (currentHealth - previousHealth) + " HP."
            );

            NotifyHealthChanged();
        }
    }

    private void NotifyHealthChanged()
    {
        Debug.Log(
            "Salud actual: " +
            currentHealth + "/" + maxHealth
        );

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("GAME OVER: el jugador se quedo sin salud.");

        OnDeath?.Invoke();
    }
}
