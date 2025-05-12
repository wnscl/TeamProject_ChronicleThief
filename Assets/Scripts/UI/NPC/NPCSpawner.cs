using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] npcPrefabs;    // 소환할 NPC 프리팹들
    public Transform[] spawnPoints;    // 소환 위치들

    private bool hasSpawned = false;   // 한 번만 소환하도록

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned) return;
        if (!other.CompareTag("Player")) return;

        // npcPrefabs.Length와 spawnPoints.Length가 같다고 가정하고,
        // i번째 프리팹을 i번째 위치에 소환합니다.
        int count = Mathf.Min(npcPrefabs.Length, spawnPoints.Length);
        for (int i = 0; i < count; i++)
        {
            Instantiate(npcPrefabs[i], spawnPoints[i].position, Quaternion.identity);
        }

        hasSpawned = true;
    }
}
