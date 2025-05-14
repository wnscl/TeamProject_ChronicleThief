using UnityEngine;

// 지정된 위치에 NPC를 스폰하고, 플레이어가 스포너 콜라이더를 밟았을 때 한 번만 소환하도록 하는 컴포넌트.
public class NPCSpawner : MonoBehaviour
{
    [Header("스폰 타이머 사용 여부")]
    [Tooltip("false면 SpawnWaveNPC 호출 시 SpawnTimer를 시작합니다.")]
    public bool disableSpawnTimer = false;

    [Header("스폰할 NPC 프리팹들")]
    [Tooltip("배열 인덱스 순서대로 spawnPoints와 매칭됩니다.")]
    public GameObject[] npcPrefabs;

    [Header("NPC 소환 위치들")]
    [Tooltip("각 NPC를 소환할 Transform 배열")]
    public Transform[] spawnPoints;

    [Header("터치 시 소환 모드")]
    [Tooltip("true면 플레이어가 콜라이더를 밟는 순간 SpawnWaveNPC를 호출합니다.")]
    public bool spawnOnTrigger = false;

    // 내부 플래그들
    private bool hasSpawned = false;      // 이미 SpawnWaveNPC() 호출 여부
    public bool PlayerTouched { get; private set; }  // 외부(BSM/UIManager)용 터치 감지 플래그

    // 애니메이터 트리거 해시
    private static readonly int SpawnReleasedHash =
        Animator.StringToHash("SpawnReleased");
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerTouched = false;
    }

    // 플레이어가 콜라이더에 들어오면 호출.
    // spawnOnTrigger 모드일 때, 한 번만 NPC를 소환.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawnOnTrigger && !hasSpawned)
        {
            // 플레이어가 스포너 위를 지나가기만 해도 여기서 SpawnWaveNPC 실행
            SpawnWaveNPC();
            PlayerTouched = true;
            Destroy(gameObject);
        }
    }

    // NPC를 실제 Instantiate 하는 메서드.
    // hasSpawned 체크로 한 번만 실행.
    public void SpawnWaveNPC()
    {
        if (hasSpawned) return;

        // npcPrefabs와 spawnPoints가 매칭된 개수만큼 순회
        for (int i = 0; i < npcPrefabs.Length && i < spawnPoints.Length; i++)
        {
            Instantiate(
                npcPrefabs[i],
                spawnPoints[i].position,
                Quaternion.identity);
        }

        hasSpawned = true;

        // 스폰 타이머 시작
        if (!disableSpawnTimer && SpawnTimer.Instance != null)
            SpawnTimer.Instance.StartCountdown(60);

        // 애니메이터 트리거(스폰 해제 애니메이션용)
        if (animator != null)
            animator.SetTrigger(SpawnReleasedHash);
    }

    // 스포너를 재활용하거나 제거하기 전에 호출.
    // 내부 상태(플래그)만 초기화.
    public void Reset()
    {
        hasSpawned = false;
        PlayerTouched = false;

        // Animator가 붙어 있을 때만 트리거 리셋
        if (animator != null)
            animator.ResetTrigger(SpawnReleasedHash);
    }
}
