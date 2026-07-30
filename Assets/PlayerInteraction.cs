using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public Camera playerCamera;
    public float interactDistance = 3f;

    private Interactable currentInteractable;

    void Update() {

        CheckInteraction();

        if (
            currentInteractable != null &&
            Input.GetKeyDown(KeyCode.E)
        ) {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction() {

        currentInteractable = null;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (
            Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactDistance
            )
        ) {

            Interactable interactable =
                hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null) {

                currentInteractable = interactable;

                UIManager.Instance.ShowInteraction(
                    interactable.interactionText
                );

                return;
            }
        }

        UIManager.Instance.HideInteraction();
    }
}
