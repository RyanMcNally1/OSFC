using UnityEngine;

public class SceneBackground : MonoBehaviour {

    [SerializeField] private Color backgroundColor =
        new Color(0.5f, 0.7f, 1f);

    void Start() {
        Camera playerCamera = Camera.main;

        if (playerCamera == null) {
            Debug.LogWarning(
                "SceneBackground could not find the Main Camera."
            );

            return;
        }

        playerCamera.clearFlags =
            CameraClearFlags.SolidColor;

        playerCamera.backgroundColor =
            backgroundColor;
    }
}
