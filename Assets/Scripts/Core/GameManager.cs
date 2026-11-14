
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Panel de pausa")]
    [SerializeField] private GameObject pausePanel;

    [Header("Paneles de resultado")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    [Header("Textos")]
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text defeatText;
    [SerializeField] private TMP_Text countdownText;

    [Header("Reinicio automatico")]
    [SerializeField] private float restartDelay = 10f;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    private float remainingRestartTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Evita que la escena comience pausada.
        Time.timeScale = 1f;

        CurrentState = GameState.Start;
    }

    private void Start()
    {
        // Ocultar todos los paneles al comenzar.
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (defeatPanel != null)
            defeatPanel.SetActive(false);

        // Conectar eventos del jugador.
        if (playerHealth != null)
        {
            playerHealth.OnDeath += HandlePlayerDeath;
            playerHealth.OnHealthChanged += HandleHealthChanged;
        }
        else
        {
            Debug.LogError(
                "GameManager: no se asigno PlayerHealth."
            );
        }

        if (playerScore != null)
        {
            playerScore.OnScoreChanged += HandleScoreChanged;
        }
        else
        {
            Debug.LogError(
                "GameManager: no se asigno PlayerScore."
            );
        }

        SetState(GameState.Playing);

        Debug.Log("Partida iniciada.");
    }

    private void Update()
    {
        // Pausar y reanudar con Escape.
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

        // Cuenta regresiva para reiniciar despues de ganar o perder.
        if (CurrentState == GameState.Victory ||
            CurrentState == GameState.GameOver)
        {
            UpdateRestartCountdown();
        }
    }

    private void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        Debug.Log("ESTADO DEL JUEGO: " + CurrentState);

        OnStateChanged?.Invoke(CurrentState);
    }

    // -------------------------
    // PAUSA
    // -------------------------

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        SetState(GameState.Paused);

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("Partida pausada.");
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;

        SetState(GameState.Playing);

        Debug.Log("Partida reanudada.");
    }

    // -------------------------
    // VICTORIA
    // -------------------------

    public void WinGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        FinishGame(GameState.Victory);
    }

    // -------------------------
    // DERROTA
    // -------------------------

    private void HandlePlayerDeath()
    {
        if (CurrentState != GameState.Playing)
            return;

        FinishGame(GameState.GameOver);
    }

    // -------------------------
    // FINALIZAR PARTIDA
    // -------------------------

    private void FinishGame(GameState finalState)
    {
        if (CurrentState != GameState.Playing)
            return;

        SetState(finalState);

        // Ocultar pausa.
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Mostrar el panel correspondiente.
        if (victoryPanel != null)
            victoryPanel.SetActive(
                finalState == GameState.Victory
            );

        if (defeatPanel != null)
            defeatPanel.SetActive(
                finalState == GameState.GameOver
            );

        int totalScore = playerScore != null
    ? playerScore.CurrentScore
    : 0;

        if (finalState == GameState.Victory && victoryText != null)
        {
            victoryText.text =
                "¡VICTORIA!\n\n" +
                "Mision completada.\n" +
                "Puntaje total: " + totalScore;
        }

        if (finalState == GameState.GameOver && defeatText != null)
        {
            defeatText.text =
                "GAME OVER\n\n" +
                "Mision fallida.\n" +
                "Puntaje total: " + totalScore;
        }


        // Preparar cuenta regresiva.
        remainingRestartTime = restartDelay;

        UpdateCountdownText();

        // Congelar el mundo.
        Time.timeScale = 0f;

        Debug.Log(
            "Partida finalizada: " + finalState +
            ". Reinicio en " + restartDelay + " segundos."
        );
    }

    // -------------------------
    // CUENTA REGRESIVA
    // -------------------------

    private void UpdateRestartCountdown()
    {
        // Usar tiempo no escalado permite contar aunque
        // Time.timeScale sea cero.
        remainingRestartTime -= Time.unscaledDeltaTime;

        remainingRestartTime =
            Mathf.Max(0f, remainingRestartTime);

        UpdateCountdownText();

        if (remainingRestartTime <= 0f)
        {
            RestartGame();
        }
    }

    private void UpdateCountdownText()
    {
        if (countdownText == null)
            return;

        int seconds = Mathf.CeilToInt(remainingRestartTime);

        countdownText.text =
            "Reiniciando partida en: " + seconds;
    }

    // -------------------------
    // DATOS DEL JUGADOR
    // -------------------------

    private void HandleHealthChanged(float current, float maximum)
    {
        Debug.Log(
            "GAME MANAGER | Salud: " +
            current + "/" + maximum
        );
    }

    private void HandleScoreChanged(int score)
    {
        Debug.Log(
            "GAME MANAGER | Puntaje: " + score
        );
    }

    // -------------------------
    // REINICIAR
    // -------------------------

    public void RestartGame()
    {
        // Evitar que se intente cargar la escena varias veces.
        if (CurrentState != GameState.Victory &&
            CurrentState != GameState.GameOver)
            return;

        Debug.Log("Reiniciando partida...");

        Time.timeScale = 1f;

        // Reactivar los coleccionables antes de recargar.
        DataCollectible.RespawnAll();

        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.buildIndex < 0)
        {
            Debug.LogError(
                "No se puede reiniciar: agrega la escena " +
                "al Build Profile de Unity."
            );

            return;
        }

        SceneManager.LoadScene(activeScene.buildIndex);
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

        Time.timeScale = 1f;
    }
}
