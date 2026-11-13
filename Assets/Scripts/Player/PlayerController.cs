
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundCheck;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;

    private bool jumpRequested;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Movimiento EXCLUSIVAMENTE con WASD.
        // Las flechas quedan reservadas para la camara.
        horizontalInput = 0f;
        verticalInput = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput -= 1f;

        if (Input.GetKey(KeyCode.D))
            horizontalInput += 1f;

        if (Input.GetKey(KeyCode.W))
            verticalInput += 1f;

        if (Input.GetKey(KeyCode.S))
            verticalInput -= 1f;

        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
        JumpPlayer();
    }

    private void MovePlayer()
    {
        if (cameraTransform == null)
            return;

        // Obtener la direccion de la camara.
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignorar la inclinacion vertical.
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Movimiento relativo a la camara.
        Vector3 movement =
            forward * verticalInput +
            right * horizontalInput;

        movement = Vector3.ClampMagnitude(movement, 1f);

        // Mantener la velocidad vertical de la fisica.
        Vector3 velocity = rb.linearVelocity;

        rb.linearVelocity = new Vector3(
            movement.x * moveSpeed,
            velocity.y,
            movement.z * moveSpeed
        );
    }

    private void RotatePlayer()
    {
        if (cameraTransform == null)
            return;

        // No rotar al jugador si no hay movimiento.
        if (horizontalInput == 0f && verticalInput == 0f)
            return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection =
            forward * verticalInput +
            right * horizontalInput;

        if (movementDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(movementDirection);

        Quaternion smoothRotation = Quaternion.Slerp(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(smoothRotation);
    }

    private void JumpPlayer()
    {
        if (!jumpRequested)
            return;

        if (isGrounded)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;

            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );
        }

        jumpRequested = false;
    }

    private void CheckGround()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            groundDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            groundCheck.position,
            groundCheck.position +
            Vector3.down * groundDistance
        );
    }
}
