
using UnityEngine;
using TMPro;

public class PlayerRaycastInteractor : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;

    [Header("LayerMask")]
    [SerializeField] private LayerMask interactionMask;

    [Header("Texto de ayuda")]
    [SerializeField] private TMP_Text currentPrompt;

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.E) &&
            currentInteractable != null)
        {
            currentInteractable.Interact();

            UpdatePromptText();
        }
    }

    private void CheckInteraction()
    {
        IInteractable detectedInteractable = null;

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
                detectedInteractable =
                    hit.collider.GetComponentInParent<IInteractable>();
            }
        }

        if (detectedInteractable != currentInteractable)
        {
            HidePrompt();

            currentInteractable = detectedInteractable;

            if (currentInteractable != null)
                ShowPrompt();
        }

        if (currentInteractable != null && currentPrompt != null)
        {
            currentPrompt.transform.rotation =
                playerCamera.transform.rotation;

            UpdatePromptText();
        }
    }

    private void ShowPrompt()
    {
        if (currentPrompt == null)
            return;

        currentPrompt.gameObject.SetActive(true);

        UpdatePromptText();
    }

    private void HidePrompt()
    {
        if (currentPrompt != null)
            currentPrompt.gameObject.SetActive(false);
    }

    private void UpdatePromptText()
    {
        if (currentPrompt == null ||
            currentInteractable == null)
            return;

        currentPrompt.text =
            currentInteractable.GetPromptText();
    }

    private void OnDisable()
    {
        HidePrompt();
        currentInteractable = null;
    }
}
