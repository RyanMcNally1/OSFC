using UnityEngine;

public class PlayerItems : MonoBehaviour {
    [Header("Bandages")]
    public int maxBandages = 3;
    public int currentBandages = 3;
    public float bandageHealAmount = 25f;

    private PlayerHealth playerHealth;

    private void Awake() {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start() {
        UIManager.Instance.UpdateBandages(currentBandages);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            UseBandage();
        }
    }

    public void UseBandage() {
        if (playerHealth == null) {
            return;
        }

        if (currentBandages <= 0) {
            Debug.Log("No bandages!");
            return;
        }

        if (playerHealth.currentHealth >= playerHealth.maxHealth) {
            Debug.Log("Already at full health!");
            return;
        }

        currentBandages--;

        playerHealth.Heal(bandageHealAmount);

        UIManager.Instance.UpdateBandages(currentBandages);

        Debug.Log(
            $"Bandage used. Health: {playerHealth.currentHealth}, " +
            $"Bandages: {currentBandages}"
        );
    }
}
