
using UnityEngine;

public class InteractablePrompt : MonoBehaviour
{
    [Header("Texto")]
    [SerializeField] private float heightOffset = 0.5f;
    [SerializeField] private int fontSize = 48;
    [SerializeField] private float characterSize = 0.08f;

    private TextMesh promptText;
    private Transform player;

    private void Awake()
    {
        GameObject textObject = new GameObject("Prompt_Text");

        textObject.transform.SetParent(transform);

        Collider objectCollider = GetComponent<Collider>();

        float height = 1.5f;

        if (objectCollider != null)
        {
            height = objectCollider.bounds.extents.y + heightOffset;
        }

        textObject.transform.localPosition = Vector3.up * height;

        promptText = textObject.AddComponent<TextMesh>();

        promptText.fontSize = fontSize;
        promptText.characterSize = characterSize;
        promptText.anchor = TextAnchor.MiddleCenter;
        promptText.alignment = TextAlignment.Center;
        promptText.color = Color.white;

        textObject.SetActive(false);
    }

    public void Show(Transform playerTransform, string message)
    {
        player = playerTransform;

        if (promptText == null)
            return;

        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }


    private void LateUpdate()
    {
        if (player == null || promptText == null)
            return;

        Vector3 direction =
            player.position - promptText.transform.position;

        // Mantener el texto derecho
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            // Invertir la orientacion para mostrar el frente del texto
            promptText.transform.rotation =
                Quaternion.LookRotation(-direction, Vector3.up);
        }
    }

}
