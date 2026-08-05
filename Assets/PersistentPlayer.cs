using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentPlayer : MonoBehaviour {

    private static PersistentPlayer instance;

    private Rigidbody playerRigidbody;

    [Header("UI Refresh")]
    public PlayerHealth playerHealth;
    public PlayerBandage playerBandage;
    public PlayerGrenadeThrower grenadeThrower;
    public PlayerFirearm rifle;
    public PlayerEquipment playerEquipment;

    void Awake() {
        if (
            instance != null &&
            instance != this
        ) {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        FindPlayerComponents();

        SceneManager.sceneLoaded +=
            OnSceneLoaded;
    }

    void FindPlayerComponents() {
        if (playerRigidbody == null) {
            playerRigidbody =
                GetComponent<Rigidbody>();
        }

        if (playerHealth == null) {
            playerHealth =
                GetComponent<PlayerHealth>();
        }

        if (playerBandage == null) {
            playerBandage =
                GetComponentInChildren<PlayerBandage>(true);
        }

        if (grenadeThrower == null) {
            grenadeThrower =
                GetComponentInChildren<PlayerGrenadeThrower>(true);
        }

        if (rifle == null) {
            rifle =
                GetComponentInChildren<PlayerFirearm>(true);
        }

        if (playerEquipment == null) {
            playerEquipment =
                GetComponent<PlayerEquipment>();
        }
    }

    void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    ) {
        StartCoroutine(
            MovePlayerToSpawn(scene)
        );
    }

    IEnumerator MovePlayerToSpawn(Scene loadedScene) {
        yield return null;

        GameObject[] spawnPoints =
            GameObject.FindGameObjectsWithTag(
                "PlayerSpawn"
            );

        Transform correctSpawnPoint = null;

        foreach (GameObject spawnPoint in spawnPoints) {
            if (
                spawnPoint.scene ==
                loadedScene
            ) {
                correctSpawnPoint =
                    spawnPoint.transform;

                break;
            }
        }

        if (correctSpawnPoint == null) {
            Debug.LogWarning(
                "No PlayerSpawn was found in scene " +
                loadedScene.name
            );

            yield break;
        }

        if (playerRigidbody != null) {
            playerRigidbody.linearVelocity =
                Vector3.zero;

            playerRigidbody.angularVelocity =
                Vector3.zero;

            playerRigidbody.position =
                correctSpawnPoint.position;

            playerRigidbody.rotation =
                correctSpawnPoint.rotation;
        }
        else {
            transform.SetPositionAndRotation(
                correctSpawnPoint.position,
                correctSpawnPoint.rotation
            );
        }

        RefreshAllUI();
    }

    void RefreshAllUI() {
        if (playerHealth != null) {
            playerHealth.RefreshUI();
        }

        if (playerBandage != null) {
            playerBandage.RefreshUI();
        }

        if (grenadeThrower != null) {
            grenadeThrower.RefreshUI();
        }

        if (rifle != null) {
            rifle.RefreshUI();
        }

        if (
            playerEquipment != null &&
            UIManager.Instance != null
        ) {
            UIManager.Instance.UpdateKitSelection(
                playerEquipment.CurrentEquipment
            );
        }
    }

    void OnDestroy() {
        if (instance != this) {
            return;
        }

        SceneManager.sceneLoaded -=
            OnSceneLoaded;

        instance = null;
    }
}
