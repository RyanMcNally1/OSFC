using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject reloadText;

    [Header("Health")]
    public Slider healthBar;

    [Header("Items")]
    public TMP_Text bandageText;

    [Header("Weapon")]
    public TMP_Text ammoText;

    [Header("Grenades")]
    public TMP_Text grenadeText;

    private void Awake() {
        Instance = this;
    }

    public void UpdateHealth(float current, float max) {
        healthBar.value = current / max;
    }

    public void UpdateBandages(int amount) {
        bandageText.text = "Bandages: " + amount;
    }

    public void UpdateAmmo(int currentAmmo, int reserveAmmo) {
        ammoText.text = "Ammo: " + currentAmmo + " / " + reserveAmmo;
    }

    public void ShowReloading(bool show) {
        reloadText.SetActive(show);
    }

    public void UpdateGrenades(int amount) {
        if (grenadeText != null) {
            grenadeText.text = "Grenades: " + amount;
        }
    }
}
