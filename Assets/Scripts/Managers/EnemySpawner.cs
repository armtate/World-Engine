
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 5;

    [Header("Spawn Safety")]
    [SerializeField] private float spawnCheckRadius = 0.5f;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning(
                "EnemySpawner: Enemy Prefab is missing!"
            );
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "EnemySpawner: No spawn points assigned!"
            );
            return;
        }

        // Stop spawning when the maximum is reached
        if (EnemyHealth.ActiveEnemyCount >= maxEnemies)
            return;

        int randomStart = Random.Range(0, spawnPoints.Length);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int index = (randomStart + i) % spawnPoints.Length;

            Transform spawnPoint = spawnPoints[index];

            if (spawnPoint == null)
                continue;

            // Prevent spawning on top of existing enemies
            Collider2D nearbyEnemy = Physics2D.OverlapCircle(
                spawnPoint.position,
                spawnCheckRadius,
                LayerMask.GetMask("Enemy")
            );

            if (nearbyEnemy != null)
                continue;

            GameObject newEnemy = Instantiate(
                enemyPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            int enemyLayer = LayerMask.NameToLayer("Enemy");

            if (enemyLayer != -1)
            {
                SetLayerRecursively(newEnemy, enemyLayer);
            }

            return;
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null)
            return;

        Gizmos.color = Color.green;

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null)
                continue;

            Gizmos.DrawWireSphere(
                spawnPoint.position,
                spawnCheckRadius
            );
        }
    }
}
