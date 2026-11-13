
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerScore playerScore;

    [Header("Textos del HUD")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        if (playerHealth == null || playerScore == null)
        {
            Debug.LogError("PlayerHUD: faltan referencias del jugador.");
            return;
        }

        // Suscribirse a los cambios de salud y puntaje
        playerHealth.OnHealthChanged += UpdateHealth;
        playerScore.OnScoreChanged += UpdateScore;

        // Mostrar los valores iniciales
        UpdateHealth(
            playerHealth.CurrentHealth,
            playerHealth.MaxHealth
        );

        UpdateScore(playerScore.CurrentScore);
    }

    private void UpdateHealth(float current, float maximum)
    {
        if (healthText != null)
        {
            healthText.text =
                "VIDA: " + current.ToString("F0") +
                " / " + maximum.ToString("F0");
        }
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "PUNTAJE: " + score;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealth;

        if (playerScore != null)
            playerScore.OnScoreChanged -= UpdateScore;
    }
}
