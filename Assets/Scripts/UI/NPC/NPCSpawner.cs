using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("스폰 타이머 사용 여부")]
    public bool disableSpawnTimer = false;

    [Header("스폰 세팅")]
    public GameObject[] npcPrefabs;
    public Transform[] spawnPoints;

    private bool hasSpawned = false;

    // Animator 파라미터 해시 (Inspector에 같은 이름의 Trigger를 만들어 두세요)
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

        // 1) NPC 소환
        int count = Mathf.Min(npcPrefabs.Length, spawnPoints.Length);
        for (int i = 0; i < count; i++)
        {
            Instantiate(npcPrefabs[i], spawnPoints[i].position, Quaternion.identity);
        }

        // 2) 소환 플래그
        hasSpawned = true;

        // 3) 카운트 다운 시작 (disableSpawnTimer가 false일 때만)
        if (!disableSpawnTimer && SpawnTimer.Instance != null)
        {
            SpawnTimer.Instance.StartCountdown(60);
        }

        // 4) 애니메이터에 ‘SpawnReleased’ 트리거
        if (animator != null)
        {
            animator.SetTrigger(SpawnReleasedHash);
        }

        // 5) 스포너 오브젝트 제거
        Destroy(gameObject);
    }
}