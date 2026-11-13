
using UnityEngine;

public class DataCollectible : MonoBehaviour
{
    [Header("Datos del recolectable")]
    [SerializeField] private int points = 25;

    [Header("Animacion")]
    [SerializeField] private float rotationSpeed = 60f;

    private bool collected;

    private void Update()
    {
        // Rotacion visual del objeto
        transform.Rotate(
            Vector3.up * rotationSpeed * Time.deltaTime,
            Space.World
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        // Buscar el sistema de puntuacion del jugador
        PlayerScore playerScore =
            other.GetComponentInParent<PlayerScore>();

        if (playerScore == null)
            return;

        collected = true;

        // Agregar puntos
        playerScore.AddPoints(points);

        Debug.Log("Dato recolectado correctamente.");

        // Eliminar el objeto
        Destroy(gameObject);
    }
}
