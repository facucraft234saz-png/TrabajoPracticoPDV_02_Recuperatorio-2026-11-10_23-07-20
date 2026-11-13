
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencias del jugador")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerScore playerScore;

    [Header("Paneles")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resultPanel;

    [Header("Textos")]
    [SerializeField] private TMP_Text resultText;

    [Header("Botones")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button retryButton;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    private bool initialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Time.timeScale = 1f;
        CurrentState = GameState.Start;

        Debug.Log("ESTADO DEL JUEGO: Start");
    }

    private void Start()
    {
        // Conectar los botones automáticamente
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveListener(ResumeGame);
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (retryButton != null)
        {
            retryButton.onClick.RemoveListener(RestartGame);
            retryButton.onClick.AddListener(RestartGame);
        }

        // Ocultar paneles inicialmente
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (resultPanel != null)
            resultPanel.SetActive(false);

        // Comprobar referencias del jugador
        if (playerHealth == null)
        {
            Debug.LogError("GameManager: falta PlayerHealth.");
            return;
        }

        if (playerScore == null)
        {
            Debug.LogError("GameManager: falta PlayerScore.");
            return;
        }

        // Suscribirse a eventos
        playerHealth.OnDeath += HandlePlayerDeath;
        playerHealth.OnHealthChanged += HandleHealthChanged;
        playerScore.OnScoreChanged += HandleScoreChanged;

        initialized = true;

        SetState(GameState.Playing);

        Debug.Log("Partida iniciada.");
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (CurrentState == GameState.Playing &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        else if (CurrentState == GameState.Paused &&
                 Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        Debug.Log("ESTADO DEL JUEGO: " + CurrentState);

        OnStateChanged?.Invoke(CurrentState);
    }

    // PAUSAR
    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        SetState(GameState.Paused);

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("Juego pausado.");
    }

    // REANUDAR
    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;

        SetState(GameState.Playing);

        Debug.Log("Juego reanudado.");
    }

    // VICTORIA
    public void WinGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        FinishGame(GameState.Victory);
    }

    // DERROTA
    private void HandlePlayerDeath()
    {
        if (CurrentState != GameState.Playing)
            return;

        FinishGame(GameState.GameOver);
    }

    // FINALIZAR PARTIDA
    private void FinishGame(GameState finalState)
    {
        if (CurrentState != GameState.Playing)
            return;

        SetState(finalState);

        // Reaparecer los recolectables
        DataCollectible.RespawnAll();

        // Ocultar pausa
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Mostrar resultados
        if (resultPanel != null)
            resultPanel.SetActive(true);

        int totalScore = playerScore.CurrentScore;

        if (resultText != null)
        {
            if (finalState == GameState.Victory)
            {
                resultText.text =
                    "¡VICTORIA!\n\n" +
                    "Mision completada.\n" +
                    "Puntaje total: " + totalScore;
            }
            else
            {
                resultText.text =
                    "GAME OVER\n\n" +
                    "Mision fallida.\n" +
                    "Puntaje total: " + totalScore;
            }
        }

        Time.timeScale = 0f;

        Debug.Log(
            "RESULTADO: " + finalState +
            " | PUNTAJE TOTAL: " + totalScore
        );
    }

    // INFORMACION DE SALUD
    private void HandleHealthChanged(float current, float maximum)
    {
        Debug.Log(
            "GAME MANAGER | Salud: " +
            current + "/" + maximum
        );
    }

    // INFORMACION DE PUNTUACION
    private void HandleScoreChanged(int score)
    {
        Debug.Log(
            "GAME MANAGER | Puntaje: " + score
        );
    }

    // REINICIAR NIVEL
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandlePlayerDeath;
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }

        if (playerScore != null)
        {
            playerScore.OnScoreChanged -= HandleScoreChanged;
        }

        if (Instance == this)
            Instance = null;
    }
}
