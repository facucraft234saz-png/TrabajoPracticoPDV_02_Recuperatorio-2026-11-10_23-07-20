
using UnityEngine;
using System.Collections;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Desaparicion")]
    [SerializeField] private float disappearDuration = 1f;

    [Header("Reaparicion")]
    [SerializeField] private float respawnTime = 20f;

    [Header("Mensajes editables")]
    [SerializeField]
    private string initialMessage = "Presiona E para eliminar";

    [SerializeField]
    private string processingMessage = "Eliminando puerta...";

    [SerializeField]
    private string finalMessage = "Puerta eliminada";

    private Vector3 originalScale;

    private bool isRunning;
    private bool isHidden;

    private float elapsedTime;

    private Coroutine doorCoroutine;

    public bool IsCompleted => isHidden;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public string GetPromptText()
    {
        if (isRunning)
        {
            float remainingTime = Mathf.Max(
                0f,
                disappearDuration - elapsedTime
            );

            return processingMessage +
                   "\nTiempo restante: " +
                   remainingTime.ToString("F1") + " s";
        }

        if (isHidden)
        {
            return finalMessage;
        }

        return initialMessage;
    }

    public void Interact()
    {
        if (isRunning || isHidden)
            return;

        doorCoroutine = StartCoroutine(DoorRoutine());
    }

    private IEnumerator DoorRoutine()
    {
        isRunning = true;
        elapsedTime = 0f;

        Debug.Log("Puerta interactuada. Comenzando eliminacion.");

        Vector3 startScale = originalScale;
        Vector3 targetScale = Vector3.zero;

        // Reducir el tamaño sin modificar la posicion.
        while (elapsedTime < disappearDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / disappearDuration
            );

            transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                progress
            );

            yield return null;
        }

        transform.localScale = Vector3.zero;

        isRunning = false;
        isHidden = true;

        Debug.Log("Puerta eliminada. Esperando para reaparecer.");

        // Esperar 20 segundos.
        yield return new WaitForSeconds(respawnTime);

        // Recuperar el tamaño original.
        transform.localScale = originalScale;

        isHidden = false;
        doorCoroutine = null;

        Debug.Log("La puerta reaparecio y puede utilizarse nuevamente.");
    }

    private void OnDisable()
    {
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
            doorCoroutine = null;
        }

        transform.localScale = originalScale;

        isRunning = false;
        isHidden = false;
        elapsedTime = 0f;
    }
}
