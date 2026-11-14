
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageInterval = 1f;

    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player == null || player.IsDead)
            return;

        if (Time.time >= nextDamageTime)
        {
            player.TakeDamage(damage);

            nextDamageTime = Time.time + damageInterval;

            Debug.Log("El laser daño al jugador.");
        }
    }
}
