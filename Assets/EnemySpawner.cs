using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [Header("Spawner")]
    public GameObject enemyPrefab;
    public int numberToSpawn = 10;
    public float spawnRadius = 8f;

    public void SpawnEnemies() {

        for (int i = 0; i < numberToSpawn; i++) {

            Vector2 randomCircle =
                Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPosition =
                transform.position +
                new Vector3(
                    randomCircle.x,
                    0f,
                    randomCircle.y
                );

            Instantiate(
                enemyPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
    }

        void Update() {

        if (Input.GetKeyDown(KeyCode.F5)) {
            SpawnEnemies();
        }
    }
}
