
using UnityEngine;
using System;

public class PlayerScore : MonoBehaviour
{
    [Header("Puntuacion")]
    [SerializeField] private int currentScore = 0;

    public int CurrentScore => currentScore;

    public event Action<int> OnScoreChanged;

    private void Start()
    {
        Debug.Log("Puntuacion inicial: " + currentScore);

        OnScoreChanged?.Invoke(currentScore);
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0)
            return;

        currentScore += amount;

        Debug.Log(
            "Puntos obtenidos: +" + amount
        );

        Debug.Log(
            "Puntuacion total: " + currentScore
        );

        OnScoreChanged?.Invoke(currentScore);
    }
}
