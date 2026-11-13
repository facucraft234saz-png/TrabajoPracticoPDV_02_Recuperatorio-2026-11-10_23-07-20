
using UnityEngine;

public class RaycastDoor : MonoBehaviour
{
    [Header("Configuracion de la puerta")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;

    private bool isOpen;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    public bool IsOpen => isOpen;

    private void Start()
    {
        closedRotation = transform.localRotation;

        openRotation = closedRotation *
            Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        Quaternion targetRotation =
            isOpen ? openRotation : closedRotation;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            openSpeed * Time.deltaTime
        );
    }

    public void Interact()
    {
        isOpen = !isOpen;

        Debug.Log(isOpen ? "Puerta abierta." : "Puerta cerrada.");
    }
}
