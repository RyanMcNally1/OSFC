using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject reloadText;

    [Header("Health")]
    public Slider healthBar;

    [Header("Weapon")]
    public TMP_Text ammoText;

    [Header("Interaction")]
    public TMP_Text interactionText;

    [Header("Actions")]
    public GameObject bandagingText;

    [Header("Equipment")]
    public TMP_Text kitText;

    [Header("Boss Health")]
    public Slider bossHealthSlider;
    public TMP_Text bossNameText;

    private int grenadeAmount;
    private int bandageAmount;

    private PlayerEquipment.EquipmentSlot selectedSlot =
        PlayerEquipment.EquipmentSlot.None;

    private void Awake() {
        Instance = this;

        if (bossHealthSlider != null) {
            bossHealthSlider.gameObject.SetActive(false);
        }

        if (bossNameText != null) {
            bossNameText.gameObject.SetActive(false);
        }
    }

    public void UpdateHealth(float current, float max) {
        healthBar.value = current / max;
    }

    public void UpdateAmmo(int currentAmmo, int reserveAmmo) {
        ammoText.text = "Ammo: " + currentAmmo + " / " + reserveAmmo;
    }

    public void ShowReloading(bool show) {
        reloadText.SetActive(show);
    }

    public void ShowBandaging(bool show) {
        if (bandagingText != null) {
            bandagingText.SetActive(show);
        }
    }

    public void UpdateGrenades(int amount) {
        grenadeAmount = amount;
        RefreshKitText();
    }

    public void UpdateBandages(int amount) {
        bandageAmount = amount;
        RefreshKitText();
    }

    public void ShowInteraction(string text) {
        interactionText.gameObject.SetActive(true);
        interactionText.text = text;
    }

    public void HideInteraction() {
        interactionText.gameObject.SetActive(false);
    }

    public void UpdateKitSelection(
        PlayerEquipment.EquipmentSlot newSelectedSlot
    ) {
        selectedSlot = newSelectedSlot;
        RefreshKitText();
    }

        private void RefreshKitText() {
        if (kitText == null) {
            return;
        }

        kitText.text =
            FormatKitEntry(
                "1. Rifle",
                selectedSlot ==
                PlayerEquipment.EquipmentSlot.Rifle
            ) +
            "\n" +
            FormatKitEntry(
                "2. Melee",
                selectedSlot ==
                PlayerEquipment.EquipmentSlot.Knife
            ) +
            "\n" +
            FormatKitEntry(
                $"3. Grenade [{grenadeAmount}]",
                selectedSlot ==
                PlayerEquipment.EquipmentSlot.Grenade
            ) +
            "\n" +
            FormatKitEntry(
                $"4. Bandage [{bandageAmount}]",
                selectedSlot ==
                PlayerEquipment.EquipmentSlot.Bandage
            );
    }

    private string FormatKitEntry(
        string label,
        bool selected
    ) {
        return selected
            ? "→ " + label
            : "   " + label;
    }

    public void ShowBossHealth(
        string bossName,
        float maxHealth
    ) {
        if (bossHealthSlider == null) {
            return;
        }

        bossHealthSlider.gameObject.SetActive(true);

        bossHealthSlider.minValue = 0f;
        bossHealthSlider.maxValue = maxHealth;
        bossHealthSlider.value = maxHealth;

        if (bossNameText != null) {
            bossNameText.gameObject.SetActive(true);
            bossNameText.text = bossName;
        }
    }

    public void UpdateBossHealth(float currentHealth) {
        if (bossHealthSlider == null) {
            return;
        }

        bossHealthSlider.value =
            Mathf.Max(0f, currentHealth);
    }

    public void HideBossHealth() {
        if (bossHealthSlider != null) {
            bossHealthSlider.gameObject.SetActive(false);
        }

        if (bossNameText != null) {
            bossNameText.gameObject.SetActive(false);
        }
    }
}
