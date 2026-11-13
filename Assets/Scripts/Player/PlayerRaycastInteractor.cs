
using UnityEngine;
using TMPro;

public class PlayerRaycastInteractor : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;

    [Header("LayerMask")]
    [SerializeField] private LayerMask interactionMask;

    private RaycastDoor currentDoor;
    private TMP_Text currentPrompt;

    private void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.E) && currentDoor != null)
        {
            currentDoor.Interact();
            UpdatePromptText();
        }
    }

    private void CheckInteraction()
    {
        RaycastDoor detectedDoor = null;

        if (playerCamera != null)
        {
            Ray ray = new Ray(
                playerCamera.transform.position,
                playerCamera.transform.forward
            );

            if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactionDistance,
                interactionMask,
                QueryTriggerInteraction.Ignore))
            {
                detectedDoor =
                    hit.collider.GetComponentInParent<RaycastDoor>();
            }
        }

        if (detectedDoor != currentDoor)
        {
            HidePrompt();

            currentDoor = detectedDoor;

            if (currentDoor != null)
                ShowPrompt();
        }

        if (currentDoor != null && currentPrompt != null)
        {
            currentPrompt.transform.rotation =
                playerCamera.transform.rotation;

            UpdatePromptText();
        }
    }

    private void ShowPrompt()
    {
        currentPrompt = currentDoor.GetComponentInChildren<TMP_Text>(true);

        if (currentPrompt != null)
        {
            currentPrompt.gameObject.SetActive(true);
            UpdatePromptText();
        }
        else
        {
            Debug.LogWarning("La puerta no tiene un texto TMP asignado.");
        }
    }

    private void HidePrompt()
    {
        if (currentPrompt != null)
            currentPrompt.gameObject.SetActive(false);

        currentPrompt = null;
    }

    private void UpdatePromptText()
    {
        if (currentPrompt == null || currentDoor == null)
            return;

        currentPrompt.text = currentDoor.IsOpen
            ? "Presiona E para cerrar"
            : "Presiona E para abrir";
    }

    private void OnDisable()
    {
        HidePrompt();
        currentDoor = null;
    }
}
