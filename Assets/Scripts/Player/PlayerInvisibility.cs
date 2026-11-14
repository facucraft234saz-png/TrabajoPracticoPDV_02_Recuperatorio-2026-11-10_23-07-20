
using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerInvisibility : MonoBehaviour
{
    [Header("Duracion")]
    [SerializeField] private float invisibilityDuration = 10f;

    [Header("Material de invisibilidad")]
    [SerializeField] private Material invisibleMaterial;

    [Header("Aumento de velocidad")]
    [SerializeField] private float speedMultiplier = 1.2f;

    [Header("HUD")]
    [SerializeField] private TMP_Text invisibilityText;

    private Renderer[] renderers;
    private Material[][] originalMaterials;

    private PlayerController playerController;

    private Coroutine invisibilityCoroutine;

    public bool IsInvisible { get; private set; }

    private float remainingTime;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].sharedMaterials;
        }

        playerController = GetComponent<PlayerController>();

        if (invisibilityText != null)
            invisibilityText.text = "";
    }

    private void Update()
    {
        if (IsInvisible)
        {
            UpdateHUD();
        }
    }

    public void ActivateInvisibility()
    {
        if (invisibilityCoroutine != null)
        {
            StopCoroutine(invisibilityCoroutine);
        }

        invisibilityCoroutine =
            StartCoroutine(InvisibilityRoutine());
    }

    private IEnumerator InvisibilityRoutine()
    {
        IsInvisible = true;
        remainingTime = invisibilityDuration;

        ApplyInvisibleMaterial();

        if (playerController != null)
        {
            playerController.SetSpeedMultiplier(speedMultiplier);
        }

        Debug.Log("Jugador invisible. Velocidad aumentada un 20%.");

        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(0f, remainingTime);

            UpdateHUD();

            yield return null;
        }

        EndInvisibility();
    }

    private void ApplyInvisibleMaterial()
    {
        if (invisibleMaterial == null)
        {
            Debug.LogWarning(
                "PlayerInvisibility: no se asigno un material invisible."
            );

            return;
        }

        foreach (Renderer currentRenderer in renderers)
        {
            if (currentRenderer == null)
                continue;

            Material[] newMaterials =
                new Material[currentRenderer.sharedMaterials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = invisibleMaterial;
            }

            currentRenderer.sharedMaterials = newMaterials;
        }
    }

    private void RestoreOriginalMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null)
                continue;

            renderers[i].sharedMaterials = originalMaterials[i];
        }
    }

    private void UpdateHUD()
    {
        if (invisibilityText == null)
            return;

        int seconds = Mathf.CeilToInt(remainingTime);

        invisibilityText.text =
            "INVISIBILIDAD: " + seconds + "s";
    }

    private void EndInvisibility()
    {
        RestoreOriginalMaterials();

        IsInvisible = false;
        remainingTime = 0f;

        if (playerController != null)
        {
            playerController.SetSpeedMultiplier(1f);
        }

        if (invisibilityText != null)
        {
            invisibilityText.text = "";
        }

        invisibilityCoroutine = null;

        Debug.Log("La invisibilidad termino. Velocidad normal restaurada.");
    }

    private void OnDisable()
    {
        if (invisibilityCoroutine != null)
        {
            StopCoroutine(invisibilityCoroutine);
            invisibilityCoroutine = null;
        }

        if (IsInvisible)
        {
            RestoreOriginalMaterials();

            if (playerController != null)
                playerController.SetSpeedMultiplier(1f);

            IsInvisible = false;
        }

        if (invisibilityText != null)
            invisibilityText.text = "";
    }
}
