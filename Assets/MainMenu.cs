using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [Header("Scenes")]
    [SerializeField] private string firstGameplayScene =
        "CastleGrounds";

    [Header("Panels")]
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject introPanel;

    void Start() {
        ShowMainMenu();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame() {
        if (mainButtons != null) {
            mainButtons.SetActive(false);
        }

        if (introPanel != null) {
            introPanel.SetActive(true);
        }
    }

    public void ShowControls() {
        if (mainButtons != null) {
            mainButtons.SetActive(false);
        }

        if (controlsPanel != null) {
            controlsPanel.SetActive(true);
        }
    }

    public void CloseIntro() {
        if (introPanel != null) {
            introPanel.SetActive(false);
        }

        if (mainButtons != null) {
            mainButtons.SetActive(true);
        }
    }

    public void ShowMainMenu() {
        if (mainButtons != null) {
            mainButtons.SetActive(true);
        }

        if (controlsPanel != null) {
            controlsPanel.SetActive(false);
        }
    }

        public void BeginGame() {
        SceneManager.LoadScene(firstGameplayScene);
    }

    public void ExitGame() {
        Debug.Log("Exiting game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying =
            false;
#else
        Application.Quit();
#endif
    }
}
