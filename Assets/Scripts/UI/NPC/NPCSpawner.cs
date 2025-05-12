using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] npcPrefabs;    // ��ȯ�� NPC �����յ�
    public Transform[] spawnPoints;    // ��ȯ ��ġ��

    private bool hasSpawned = false;   // �� ���� ��ȯ�ϵ���

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned) return;
        if (!other.CompareTag("Player")) return;

        // npcPrefabs.Length�� spawnPoints.Length�� ���ٰ� �����ϰ�,
        // i��° �������� i��° ��ġ�� ��ȯ�մϴ�.
        int count = Mathf.Min(npcPrefabs.Length, spawnPoints.Length);
        for (int i = 0; i < count; i++)
        {
            Instantiate(npcPrefabs[i], spawnPoints[i].position, Quaternion.identity);
        }

        hasSpawned = true;
    }
}
