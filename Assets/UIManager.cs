using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Slider healthBar;
    public TMP_Text bandageText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(float current, float max)
    {
        healthBar.value = current / max;
    }

    public void UpdateBandages(int amount)
    {
        bandageText.text = "Bandages: " + amount;
    }
}
