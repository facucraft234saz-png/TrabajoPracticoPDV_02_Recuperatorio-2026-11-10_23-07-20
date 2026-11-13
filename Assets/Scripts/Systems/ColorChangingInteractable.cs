
using UnityEngine;
using System.Collections;

public class ColorChangingInteractable : MonoBehaviour, IInteractable
{
    [Header("Duracion")]
    [SerializeField] private float duration = 10f;

    [Header("Colores")]
    [SerializeField] private Color startColor = Color.red;
    [SerializeField] private Color finalColor = Color.green;

    public bool IsCompleted => hasCompleted;

    [Header("Mensajes editables")]
    [SerializeField]
    private string initialMessage =
        "Presiona E para manipular";

    [SerializeField]
    private string processingMessage =
        "Procesando...";

    [SerializeField]
    private string finalMessage =
        "Proceso completado";

    private Renderer objectRenderer;

  

    private bool isRunning;
    private bool hasCompleted;

    private float elapsedTime;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("El bloque necesita un Renderer.");
            return;
        }

        objectRenderer.material.color = startColor;
    }

    public string GetPromptText()
    {
        if (hasCompleted)
        {
            return finalMessage;
        }

        if (isRunning)
        {
            float remainingTime = Mathf.Max(
                0f,
                duration - elapsedTime
            );

            return processingMessage +
                   "\nTiempo restante: " +
                   remainingTime.ToString("F1") + " s";
        }

        return initialMessage;
    }

    public void Interact()
    {
        if (isRunning || hasCompleted)
            return;

        if (objectRenderer == null)
            return;

        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        isRunning = true;
        elapsedTime = 0f;

        Debug.Log("Bloque interactivo activado.");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / duration
            );

            objectRenderer.material.color = Color.Lerp(
                startColor,
                finalColor,
                progress
            );

            yield return null;
        }

        objectRenderer.material.color = finalColor;

        isRunning = false;
        hasCompleted = true;

        Debug.Log(finalMessage);
    }
}
