using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnerLateral : MonoBehaviour
{
    public Transform player;
    public GameObject normalPlatformPrefab;
    public GameObject disappearingPlatformPrefab;
    public GameObject enemyPrefab;

    public float startX = 6f;
    public float startY = -1.1f;

    public float minGapX = 2.2f;
    public float maxGapX = 2.8f;

    public float minY = -1.2f;
    public float maxY = 0.5f;
    public float minYStep = -0.1f;
    public float maxYStep = 0.1f;

    public int maxPlatforms = 12;
    public float destroyDistance = 15f;

    [Range(0f, 1f)] public float disappearingChance = 0.25f;
    [Range(0f, 1f)] public float enemyChance = 0.2f;

    public float minEnemySpawnDistanceFromPlayer = 14f;

    public int platformsBeforeEnemies = 5;

    public float difficultyInterval = 10f;
    public float gapIncrease = 0.15f;
    public float enemyChanceIncrease = 0.05f;
    public float disappearingChanceIncrease = 0.05f;
    public float maxEnemyChance = 0.5f;
    public float maxDisappearingChance = 0.6f;

    private float currentX;
    private float currentY;
    private int totalPlatformsSpawned = 0;

    private List<GameObject> spawnedPlatforms = new List<GameObject>();

    void Start()
    {
        currentX = startX;
        currentY = startY;

        for (int i = 0; i < 6; i++)
        {
            SpawnPlatform();
        }

        StartCoroutine(DifficultyLoop());
    }
    void Update()
    {
        while (player != null && currentX < player.position.x + 20f)
        {
            SpawnPlatform();
        }

        CleanupPlatforms();
    }
    void SpawnPlatform()
    {
        GameObject chosenPlatform;

        if (Random.value < disappearingChance && disappearingPlatformPrefab != null)
            chosenPlatform = disappearingPlatformPrefab;
        else
            chosenPlatform = normalPlatformPrefab;

        float yStep = Random.Range(minYStep, maxYStep);
        currentY += yStep;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        Vector3 spawnPos = new Vector3(currentX, currentY, 0f);

        GameObject newPlatform = Instantiate(chosenPlatform, spawnPos, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        totalPlatformsSpawned++;
        TrySpawnEnemy(newPlatform);

        currentX += Random.Range(minGapX, maxGapX);

        if (spawnedPlatforms.Count > maxPlatforms)
        {
            if (spawnedPlatforms[0] != null)
                Destroy(spawnedPlatforms[0]);

            spawnedPlatforms.RemoveAt(0);
        }
    }
    void TrySpawnEnemy(GameObject platform)
{
    if (enemyPrefab == null) return;
    if (player == null) return;

    if (totalPlatformsSpawned <= platformsBeforeEnemies)
        return;

    if (Random.value > enemyChance)
        return;

    Vector3 enemyPos = platform.transform.position + new Vector3(0f, 1.5f, 0f);

    float distanceX = Mathf.Abs(enemyPos.x - player.position.x);

    if (distanceX < minEnemySpawnDistanceFromPlayer)
        return;

    Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
}
    void CleanupPlatforms()
    {
        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i] == null)
            {
                spawnedPlatforms.RemoveAt(i);
                continue;
            }

            if (player != null && spawnedPlatforms[i].transform.position.x < player.position.x - destroyDistance)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
    IEnumerator DifficultyLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyInterval);

            maxGapX += gapIncrease;

            enemyChance = Mathf.Min(enemyChance + enemyChanceIncrease, maxEnemyChance);
            disappearingChance = Mathf.Min(disappearingChance + disappearingChanceIncrease, maxDisappearingChance);
        }
    }
}