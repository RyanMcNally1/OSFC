using UnityEngine;

public class SupplyCrate : Interactable {

    [Header("Crate Settings")]
    public bool destroyAfterUse = false;
    public GameObject closedCrateVisual;
    public GameObject openedCrateVisual;

    private bool hasBeenUsed = false;

    public override void Interact() {
        if (hasBeenUsed) {
            return;
        }

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null) {
            Debug.LogWarning(
                "Supply crate could not find the Player."
            );

            return;
        }

        RefillPlayerItems(playerObject);

        hasBeenUsed = true;
        interactionText = "Empty";

        if (closedCrateVisual != null) {
            closedCrateVisual.SetActive(false);
        }

        if (openedCrateVisual != null) {
            openedCrateVisual.SetActive(true);
        }

        if (destroyAfterUse) {
            Destroy(gameObject);
        }
    }

    void RefillPlayerItems(GameObject playerObject) {
        PlayerFirearm firearm =
            playerObject.GetComponentInChildren<PlayerFirearm>(
                true
            );

        PlayerGrenadeThrower grenadeThrower =
            playerObject.GetComponentInChildren<PlayerGrenadeThrower>(
                true
            );

        PlayerBandage bandages =
            playerObject.GetComponentInChildren<PlayerBandage>(
                true
            );

        if (firearm != null) {
            firearm.RefillAmmo();
        }

        if (grenadeThrower != null) {
            grenadeThrower.RefillGrenades();
        }

        if (bandages != null) {
            bandages.RefillBandages();
        }

        Debug.Log(
            "supplies were refilled."
        );
    }
}
