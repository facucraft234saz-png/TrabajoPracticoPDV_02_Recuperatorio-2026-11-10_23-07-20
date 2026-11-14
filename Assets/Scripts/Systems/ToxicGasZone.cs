
using UnityEngine;

public class ToxicGasZone : MonoBehaviour
{
    [Header("Daño del gas")]
    [SerializeField] private float damagePerSecond = 1f;

    private float damageTimer;
    private PlayerHealth playerInside;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            playerInside = player;
            damageTimer = 0f;

            Debug.Log("Jugador entro en la zona de gas toxico.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerInside == null)
            return;

        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != playerInside)
            return;

        damageTimer += Time.deltaTime;

        if (damageTimer >= 1f)
        {
            damageTimer -= 1f;

            playerInside.TakeEnvironmentalDamage(
                damagePerSecond
            );

            Debug.Log("Gas toxico: -1 HP.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player == playerInside)
        {
            playerInside = null;
            damageTimer = 0f;

            Debug.Log("Jugador salio de la zona de gas.");
        }
    }
}
