using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvancedEnemySpawner : MonoBehaviour
{
    [Header("Настройки спауна")]
    [SerializeField] private List<EnemySpawnGroup> spawnGroups = new List<EnemySpawnGroup>();
    [SerializeField] private float delayBetweenGroups = 1f;

    [Header("Настройки триггера")]
    [SerializeField] private Collider2D spawnTrigger;


    public bool triggered = false;

    [System.Serializable]
    public class EnemySpawnGroup
    {
        public GameObject enemyPrefab;
        public List<Transform> spawnPoints = new List<Transform>();
        public int spawnCount = 1;
        public float delayBetweenSpawns = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        other.GetComponent<ItemPickupSystem>().NonCheckedEnemyTriggers.Add(this);
        triggered = true;
        StartCoroutine(SpawnEnemiesRoutine());

        
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        foreach (var group in spawnGroups)
        {
            yield return StartCoroutine(SpawnGroup(group));
            yield return new WaitForSeconds(delayBetweenGroups);
        }
    }

    private IEnumerator SpawnGroup(EnemySpawnGroup group)
    {
        if (group.enemyPrefab == null || group.spawnPoints.Count == 0) yield break;

        for (int i = 0; i < group.spawnCount; i++)
        {
            foreach (var point in group.spawnPoints)
            {
                if (point != null)
                {
                    Instantiate(group.enemyPrefab, point.position, Quaternion.identity);
                }
            }

            if (i < group.spawnCount - 1)
            {
                yield return new WaitForSeconds(group.delayBetweenSpawns);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnTrigger != null)
        {
            Gizmos.color = triggered ? Color.yellow : Color.green;
            Gizmos.DrawWireCube(spawnTrigger.bounds.center, spawnTrigger.bounds.size);
        }

        foreach (var group in spawnGroups)
        {
            if (group.enemyPrefab == null) continue;

            foreach (var point in group.spawnPoints)
            {
                if (point == null) continue;

                Gizmos.color = Color.Lerp(Color.blue, Color.red,
                    (float)spawnGroups.IndexOf(group) / spawnGroups.Count);
                Gizmos.DrawSphere(point.position, 0.3f);
                Gizmos.DrawWireCube(point.position, Vector3.one * 0.5f);
                Gizmos.DrawLine(point.position, spawnTrigger.bounds.center);
            }
        }
    }
}