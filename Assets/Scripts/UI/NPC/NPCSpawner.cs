using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] npcPrefabs;
    public Transform[] spawnPoints;

    private bool hasSpawned = false;

    // Animator �Ķ���� �ؽ� (Inspector�� ���� �̸��� Trigger�� ����� �μ���)
    private static readonly int SpawnReleasedHash = Animator.StringToHash("SpawnReleased");
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned) return;
        if (!other.CompareTag("Player")) return;

        // 1) NPC ��ȯ
        int count = Mathf.Min(npcPrefabs.Length, spawnPoints.Length);
        for (int i = 0; i < count; i++)
        {
            Instantiate(npcPrefabs[i], spawnPoints[i].position, Quaternion.identity);
        }

        // 2) ��ȯ �÷���
        hasSpawned = true;

        // 3) �ִϸ����Ϳ� ��SpawnReleased�� Ʈ����
        if (animator != null)
            animator.SetTrigger(SpawnReleasedHash);
    }
}