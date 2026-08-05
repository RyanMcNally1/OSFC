using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject winPanel;

    [Header("Scenes")]
    [SerializeField] private string castleGroundsScene =
        "CastleGrounds";

    [Header("Player Input Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerFirearm playerFirearm;
    [SerializeField] private PlayerKnife playerKnife;
    [SerializeField] private PlayerGrenadeThrower grenadeThrower;
    [SerializeField] private PlayerBandage playerBandage;
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private PlayerInteraction playerInteraction;

    private bool isPaused;
    private bool isDead;
    private bool hasWon;
    private bool isRestarting;

    void Start() {
        FindPlayerScripts();

        if (pausePanel != null) {
            pausePanel.SetActive(false);
        }

        if (deathPanel != null) {
            deathPanel.SetActive(false);
        }

        if (winPanel != null) {
            winPanel.SetActive(false);
        }

        SetPaused(false);
    }

    void Update() {
        if (
            !isDead &&
            !isRestarting &&
            !hasWon &&
            Input.GetKeyDown(KeyCode.Escape)
        ) {
            TogglePause();
        }
    }

    void FindPlayerScripts() {
        playerController =
            FindAnyObjectByType<PlayerController>();

        playerFirearm =
            FindAnyObjectByType<PlayerFirearm>();

        playerKnife =
            FindAnyObjectByType<PlayerKnife>();

        grenadeThrower =
            FindAnyObjectByType<PlayerGrenadeThrower>();

        playerBandage =
            FindAnyObjectByType<PlayerBandage>();

        playerEquipment =
            FindAnyObjectByType<PlayerEquipment>();

        playerInteraction =
            FindAnyObjectByType<PlayerInteraction>();
    }

    public void TogglePause() {
        if (isDead || hasWon || isRestarting) {
            return;
        }

        if (!isPaused) {
            FindPlayerScripts();
        }

        SetPaused(!isPaused);
    }

    public void ResumeGame() {
        SetPaused(false);
    }

    public void RestartGame() {
        if (isRestarting) {
            return;
        }

        isRestarting = true;

        Time.timeScale = 1f;

        SetGameplayScriptsEnabled(false);

        if (pausePanel != null) {
            pausePanel.SetActive(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PersistentPlayer persistentPlayer =
            FindAnyObjectByType<PersistentPlayer>();

        if (persistentPlayer != null) {
            persistentPlayer.PrepareForRestart();
        }

        SceneManager.LoadScene(
            castleGroundsScene
        );
    }

    public void ReturnToMainMenu() {
        Time.timeScale = 1f;

        SetGameplayScriptsEnabled(false);

        PersistentPlayer persistentPlayer =
            FindAnyObjectByType<PersistentPlayer>();

        if (persistentPlayer != null) {
            persistentPlayer.PrepareForRestart();
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void ShowDeathScreen() {
        if (isDead) {
            return;
        }

        isDead = true;
        isPaused = false;

        SetGameplayScriptsEnabled(false);

        if (pausePanel != null) {
            pausePanel.SetActive(false);
        }

        if (deathPanel != null) {
            deathPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowWinScreen() {
        if (hasWon) {
            return;
        }

        hasWon = true;
        isPaused = false;

        SetGameplayScriptsEnabled(false);

        if (pausePanel != null) {
            pausePanel.SetActive(false);
        }

        if (deathPanel != null) {
            deathPanel.SetActive(false);
        }

        if (winPanel != null) {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitGame() {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetPaused(bool paused) {
        isPaused = paused;

        if (pausePanel != null) {
            pausePanel.SetActive(paused);
        }

        Time.timeScale = paused
            ? 0f
            : 1f;

        SetGameplayScriptsEnabled(
            !paused
        );

        Cursor.lockState = paused
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = paused;
    }

    private void SetGameplayScriptsEnabled(
        bool scriptsEnabled
    ) {
        if (playerController != null) {
            playerController.enabled =
                scriptsEnabled;
        }

        if (playerFirearm != null) {
            playerFirearm.enabled =
                scriptsEnabled;
        }

        if (playerKnife != null) {
            playerKnife.enabled =
                scriptsEnabled;
        }

        if (grenadeThrower != null) {
            grenadeThrower.enabled =
                scriptsEnabled;
        }

        if (playerBandage != null) {
            playerBandage.enabled =
                scriptsEnabled;
        }

        if (playerEquipment != null) {
            playerEquipment.enabled =
                scriptsEnabled;
        }

        if (playerInteraction != null) {
            playerInteraction.enabled =
                scriptsEnabled;
        }
    }

    void OnDestroy() {
        Time.timeScale = 1f;
    }
}
