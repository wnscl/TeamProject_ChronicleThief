using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    private bool hasSpawned;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
            hasSpawned = true;
        }
    }
}