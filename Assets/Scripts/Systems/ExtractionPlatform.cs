
using UnityEngine;

public class ExtractionPlatform : MonoBehaviour
{
    [Header("Bloque que debe ser hackeado")]
    [SerializeField]
    private ColorChangingInteractable targetBlock;

    [Header("Mensajes editables")]
    [SerializeField]
    private string initialMessage =
        "Hackea el Bloque Interactivo y vuelve a la plataforma.";

    [SerializeField]
    private string finalMessage =
        "Extraccion autorizada";

    [Header("Apariencia del texto")]
    [SerializeField]
    private int fontSize = 42;

    [SerializeField]
    private float characterSize = 0.07f;

    [SerializeField]
    private float textHeight = 2f;

    [SerializeField]
    private Color textColor = Color.white;

    private TextMesh platformText;

    private bool playerInside;

    private void Awake()
    {
        CreateMessage();
    }

    private void CreateMessage()
    {
        GameObject textObject =
            new GameObject("Extraction_Message");

        textObject.transform.SetParent(transform);

        textObject.transform.localPosition =
            Vector3.up * textHeight;

        platformText = textObject.AddComponent<TextMesh>();

        platformText.fontSize = fontSize;
        platformText.characterSize = characterSize;

        platformText.anchor = TextAnchor.MiddleCenter;
        platformText.alignment = TextAlignment.Center;

        platformText.color = textColor;
        platformText.text = initialMessage;

        textObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player == null)
            return;

        if (player.IsDead)
            return;

        playerInside = true;

        if (targetBlock == null)
        {
            Debug.LogError(
                "ExtractionPlatform: falta asignar el bloque hackeable."
            );

            ShowMessage("Error: falta asignar el bloque.");
            return;
        }

        CheckExtraction();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player == null)
            return;

        playerInside = false;

        HideMessage();
    }

    private void CheckExtraction()
    {
        if (targetBlock == null)
            return;

        if (!targetBlock.IsCompleted)
        {
            ShowMessage(initialMessage);

            Debug.Log(
                "Extraccion bloqueada: primero debes hackear la computadora."
            );

            return;
        }

        ShowMessage(finalMessage);

        Debug.Log("Extraccion autorizada. Victoria.");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            Debug.LogError(
                "No se encontro el GameManager en la escena."
            );
        }
    }

    private void ShowMessage(string message)
    {
        if (platformText == null)
            return;

        platformText.text = message;
        platformText.gameObject.SetActive(true);
    }

    private void HideMessage()
    {
        if (platformText != null)
        {
            platformText.gameObject.SetActive(false);
        }
    }


    private void LateUpdate()
    {
        if (!playerInside)
            return;

        if (platformText == null ||
            !platformText.gameObject.activeSelf)
            return;

        if (Camera.main == null)
            return;

        // Obtener la direccion horizontal hacia la camara.
        Vector3 direction =
            platformText.transform.position -
            Camera.main.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        // Orientar el texto hacia la camara.
        Quaternion rotation =
            Quaternion.LookRotation(direction.normalized, Vector3.up);

        // Corregir la orientacion frontal del TextMesh.
        platformText.transform.rotation =
            rotation * Quaternion.Euler(0f, 180f, 0f);
    }

}

