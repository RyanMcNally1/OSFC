using UnityEngine;

public class KingInteraction : Interactable {

    private bool hasBeenCaptured;

    public override void Interact() {
        if (hasBeenCaptured) {
            return;
        }

        hasBeenCaptured = true;

        Debug.Log(
            "The player captured the king. Game won."
        );

        // Add the real win-screen behavior later.
        ActivateWin();
    }

    void ActivateWin() {
        Debug.Log(
            "Win sequence activated."
        );
    }
}
