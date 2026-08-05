using UnityEngine;

public class KingInteraction : Interactable {

    private bool hasBeenCaptured;

    public override void Interact() {
        if (hasBeenCaptured) {
            return;
        }

        hasBeenCaptured = true;

        PauseMenu pauseMenu =
            FindAnyObjectByType<PauseMenu>();

        if (pauseMenu != null) {
            pauseMenu.ShowWinScreen();
        }
        else {
            Debug.LogWarning(
                "KingInteraction could not find PauseMenu."
            );
        }
    }
}
