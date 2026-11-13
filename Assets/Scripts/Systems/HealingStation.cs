
using UnityEngine;

public class HealingStation : MonoBehaviour
{
    [Header("Curacion")]
    [SerializeField] private float healAmount = 10f;
    [SerializeField] private float healInterval = 1.5f;

    [Header("Carga de la estacion")]
    [SerializeField] private int maxCharges = 5;
    [SerializeField] private float cooldownTime = 10f;

    [Header("Efectos visuales")]
    [SerializeField] private Light stationLight;
    [SerializeField] private ParticleSystem healingParticles;

    private int currentCharges;
    private float healTimer;
    private float cooldownTimer;

    private PlayerHealth playerInside;

    private bool isCoolingDown;

    private void Start()
    {
        currentCharges = maxCharges;

        SetStationActive(true);

        Debug.Log("Estacion de curacion activada.");
    }

    private void Update()
    {
        // Si la estacion esta recargando.
        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                FinishCooldown();
            }

            return;
        }

        // Si no hay jugador dentro, no cura.
        if (playerInside == null)
            return;

        // No consumir carga si el jugador ya tiene toda su vida.
        if (playerInside.CurrentHealth >= playerInside.MaxHealth)
            return;

        healTimer += Time.deltaTime;

        if (healTimer >= healInterval)
        {
            healTimer = 0f;

            playerInside.Heal(healAmount);

            currentCharges--;

            Debug.Log(
                "Estacion: curacion realizada. Cargas restantes: "
                + currentCharges
            );

            if (currentCharges <= 0)
            {
                StartCooldown();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health =
            other.GetComponentInParent<PlayerHealth>();

        if (health != null)
        {
            playerInside = health;
            healTimer = 0f;

            Debug.Log("Jugador dentro de la estacion de curacion.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerHealth health =
            other.GetComponentInParent<PlayerHealth>();

        if (health != null && health == playerInside)
        {
            playerInside = null;
            healTimer = 0f;

            Debug.Log("Jugador salio de la estacion.");
        }
    }

    private void StartCooldown()
    {
        isCoolingDown = true;
        cooldownTimer = cooldownTime;

        SetStationActive(false);

        Debug.Log(
            "Estacion agotada. Recargando durante "
            + cooldownTime + " segundos."
        );
    }

    private void FinishCooldown()
    {
        isCoolingDown = false;
        currentCharges = maxCharges;
        healTimer = 0f;

        SetStationActive(true);

        Debug.Log("Estacion recargada y disponible.");
    }

    private void SetStationActive(bool active)
    {
        if (stationLight != null)
            stationLight.enabled = active;

        if (healingParticles != null)
        {
            if (active)
                healingParticles.Play();
            else
                healingParticles.Stop();
        }
    }
}
