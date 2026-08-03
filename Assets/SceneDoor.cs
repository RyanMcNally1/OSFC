using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : Interactable
{
    [Header("Scene Transition")]
    [SerializeField] private string sceneToLoad = "CastleInterior";

    public override void Interact()
    {
        if (string.IsNullOrWhiteSpace(sceneToLoad))
        {
            Debug.LogWarning(
                gameObject.name +
                " does not have a scene assigned."
            );

            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
