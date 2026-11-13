
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Camara")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float lookHeight = 1.2f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float followSpeed = 8f;

    private float yaw;

    private void Start()
    {
        yaw = target.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetKey(KeyCode.LeftArrow))
            yaw -= rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
            yaw += rotationSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);

        Vector3 lookPoint =
            target.position + Vector3.up * lookHeight;

        Vector3 desiredPosition =
            lookPoint - rotation * Vector3.forward * distance
            + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        transform.LookAt(lookPoint);
    }
}
