
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DroneVision : MonoBehaviour
{
    [Header("Rango de vision")]
    [SerializeField] private float visionRadius = 6f;

    [Header("Altura respecto al dron")]
    [SerializeField] private float visionHeight = 1.5f;

    private DroneAI drone;
    private SphereCollider visionCollider;

    private void Awake()
    {
        drone = GetComponentInParent<DroneAI>();
        visionCollider = GetComponent<SphereCollider>();

        ConfigureVision();
    }

    private void OnValidate()
    {
        visionRadius = Mathf.Max(0.5f, visionRadius);

        if (visionCollider == null)
            visionCollider = GetComponent<SphereCollider>();

        ConfigureVision();
    }

    private void ConfigureVision()
    {
        if (visionCollider == null)
            return;

        // El objeto VisionRange conserva una escala uniforme.
        transform.localScale = Vector3.one;

        // La altura se controla desde la posicion local.
        transform.localPosition = new Vector3(
            0f,
            visionHeight,
            0f
        );

        // El collider tiene el radio real de deteccion.
        visionCollider.center = Vector3.zero;
        visionCollider.radius = visionRadius;
        visionCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (drone == null)
            return;

        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null)
            drone.SetTarget(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (drone == null)
            return;

        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null)
            drone.RemoveTarget(player);
    }
}
