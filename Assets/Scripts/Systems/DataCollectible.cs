
using UnityEngine;
using System.Collections.Generic;

public class DataCollectible : MonoBehaviour
{
    private static readonly List<DataCollectible> allCollectibles =
        new List<DataCollectible>();

    [Header("Datos")]
    [SerializeField] private int points = 25;

    [Header("Animacion")]
    [SerializeField] private float rotationSpeed = 60f;

    private bool collected;

    private void Awake()
    {
        if (!allCollectibles.Contains(this))
        {
            allCollectibles.Add(this);
        }
    }

    private void Update()
    {
        if (!collected)
        {
            transform.Rotate(
                Vector3.up * rotationSpeed * Time.deltaTime,
                Space.World
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PlayerScore score =
            other.GetComponentInParent<PlayerScore>();

        if (score == null)
            return;

        collected = true;

        score.AddPoints(points);

        Debug.Log("Dato recolectado: +" + points);

        // Ocultar, pero no destruir
        gameObject.SetActive(false);
    }

    public static void RespawnAll()
    {
        foreach (DataCollectible collectible in allCollectibles)
        {
            if (collectible == null)
                continue;

            collectible.collected = false;
            collectible.gameObject.SetActive(true);
        }

        Debug.Log("Todos los Data Collectible reaparecieron.");
    }

    private void OnDestroy()
    {
        allCollectibles.Remove(this);
    }
}
