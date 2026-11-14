
using UnityEngine;
using System.Collections;

public class InvisibilityPowerUp : MonoBehaviour
{
    [Header("Animacion")]
    [SerializeField] private float rotationSpeed = 60f;

    [Header("Reaparicion")]
    [SerializeField] private float respawnTime = 20f;

    private bool collected;

    private Renderer[] powerUpRenderers;
    private Collider[] powerUpColliders;

    private bool[] originalRendererStates;
    private bool[] originalColliderStates;

    private void Awake()
    {
        powerUpRenderers =
            GetComponentsInChildren<Renderer>(true);

        powerUpColliders =
            GetComponentsInChildren<Collider>(true);

        originalRendererStates =
            new bool[powerUpRenderers.Length];

        originalColliderStates =
            new bool[powerUpColliders.Length];

        for (int i = 0; i < powerUpRenderers.Length; i++)
        {
            originalRendererStates[i] =
                powerUpRenderers[i].enabled;
        }

        for (int i = 0; i < powerUpColliders.Length; i++)
        {
            originalColliderStates[i] =
                powerUpColliders[i].enabled;
        }
    }

    private void Update()
    {
        if (!collected)
        {
            transform.Rotate(
                Vector3.up * rotationSpeed * Time.deltaTime,
                Space.World
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PlayerInvisibility invisibility =
            other.GetComponentInParent<PlayerInvisibility>();

        if (invisibility == null)
            return;

        collected = true;

        invisibility.ActivateInvisibility();

        HidePowerUp();

        Debug.Log(
            "Power-up de invisibilidad recolectado. Reaparecera en "
            + respawnTime + " segundos."
        );

        StartCoroutine(RespawnRoutine());
    }

    private void HidePowerUp()
    {
        foreach (Renderer currentRenderer in powerUpRenderers)
        {
            if (currentRenderer != null)
                currentRenderer.enabled = false;
        }

        foreach (Collider currentCollider in powerUpColliders)
        {
            if (currentCollider != null)
                currentCollider.enabled = false;
        }
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        for (int i = 0; i < powerUpRenderers.Length; i++)
        {
            if (powerUpRenderers[i] != null)
            {
                powerUpRenderers[i].enabled =
                    originalRendererStates[i];
            }
        }

        for (int i = 0; i < powerUpColliders.Length; i++)
        {
            if (powerUpColliders[i] != null)
            {
                powerUpColliders[i].enabled =
                    originalColliderStates[i];
            }
        }

        collected = false;

        Debug.Log("Power-up de invisibilidad reaparecido.");
    }
}
