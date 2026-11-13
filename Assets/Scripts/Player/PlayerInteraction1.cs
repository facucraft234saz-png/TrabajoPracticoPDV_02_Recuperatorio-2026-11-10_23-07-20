
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Rango")]
    [SerializeField] private float interactionRange = 3f;

    [Header("Deteccion")]
    [SerializeField] private LayerMask interactableLayer;

    [Header("Tecla")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private InteractablePrompt currentPrompt;
    private IInteractable currentInteractable;
    private Collider currentCollider;

    private void Update()
    {
        DetectInteractables();

        if (Input.GetKeyDown(interactionKey))
        {
            TryInteract();
        }
    }

    private void DetectInteractables()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(
            transform.position + Vector3.up,
            interactionRange,
            interactableLayer,
            QueryTriggerInteraction.Ignore
        );

        InteractablePrompt nearestPrompt = null;
        IInteractable nearestInteractable = null;
        Collider nearestCollider = null;

        float nearestDistance = float.MaxValue;

        foreach (Collider detectedCollider in nearbyObjects)
        {
            MonoBehaviour[] components =
                detectedCollider.GetComponentsInParent<MonoBehaviour>();

            IInteractable interactable = null;
            InteractablePrompt prompt = null;

            foreach (MonoBehaviour component in components)
            {
                if (interactable == null)
                    interactable = component as IInteractable;

                if (prompt == null)
                    prompt = component as InteractablePrompt;
            }

            if (interactable == null || prompt == null)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                detectedCollider.bounds.center
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPrompt = prompt;
                nearestInteractable = interactable;
                nearestCollider = detectedCollider;
            }
        }

        if (currentPrompt != null && currentPrompt != nearestPrompt)
        {
            currentPrompt.Hide();
        }

        currentPrompt = nearestPrompt;
        currentInteractable = nearestInteractable;
        currentCollider = nearestCollider;

        if (currentPrompt != null)
        {
            currentPrompt.Show(
                transform,
                currentInteractable.GetPromptText()
            );
        }
    }

    private void TryInteract()
    {
        if (currentInteractable == null || currentCollider == null)
        {
            Debug.Log("No hay objetos interactivos en rango.");
            return;
        }

        Vector3 origin = transform.position + Vector3.up;

        Vector3 targetPosition = currentCollider.bounds.center;

        Vector3 direction = targetPosition - origin;

        float distance = direction.magnitude;

        if (distance > interactionRange + 0.2f)
            return;

        // Raycast desde el jugador hacia el objeto.
        // No depende de la camara ni de la orientacion del jugador.
        if (Physics.Raycast(
            origin,
            direction.normalized,
            out RaycastHit hit,
            distance + 0.2f,
            interactableLayer,
            QueryTriggerInteraction.Ignore))
        {
            if (hit.collider == currentCollider)
            {
                currentInteractable.Interact();
                Debug.Log("Interaccion ejecutada correctamente.");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position + Vector3.up,
            interactionRange
        );
    }
}
