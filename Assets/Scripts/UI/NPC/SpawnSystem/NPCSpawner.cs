using UnityEngine;

// ������ ��ġ�� NPC�� �����ϰ�, �÷��̾ ������ �ݶ��̴��� ����� �� �� ���� ��ȯ�ϵ��� �ϴ� ������Ʈ.
public class NPCSpawner : MonoBehaviour
{
    [Header("���� Ÿ�̸� ��� ����")]
    [Tooltip("false�� SpawnWaveNPC ȣ�� �� SpawnTimer�� �����մϴ�.")]
    public bool disableSpawnTimer = false;

    [Header("������ NPC �����յ�")]
    [Tooltip("�迭 �ε��� ������� spawnPoints�� ��Ī�˴ϴ�.")]
    public GameObject[] npcPrefabs;

    [Header("NPC ��ȯ ��ġ��")]
    [Tooltip("�� NPC�� ��ȯ�� Transform �迭")]
    public Transform[] spawnPoints;

    [Header("��ġ �� ��ȯ ���")]
    [Tooltip("true�� �÷��̾ �ݶ��̴��� ��� ���� SpawnWaveNPC�� ȣ���մϴ�.")]
    public bool spawnOnTrigger = false;

    // ���� �÷��׵�
    private bool hasSpawned = false;      // �̹� SpawnWaveNPC() ȣ�� ����
    public bool PlayerTouched { get; private set; }  // �ܺ�(BSM/UIManager)�� ��ġ ���� �÷���

    // �ִϸ����� Ʈ���� �ؽ�
    private static readonly int SpawnReleasedHash =
        Animator.StringToHash("SpawnReleased");
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerTouched = false;
    }

    // �÷��̾ �ݶ��̴��� ������ ȣ��.
    // spawnOnTrigger ����� ��, �� ���� NPC�� ��ȯ.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawnOnTrigger && !hasSpawned)
        {
            // �÷��̾ ������ ���� �������⸸ �ص� ���⼭ SpawnWaveNPC ����
            SpawnWaveNPC();
            PlayerTouched = true;
            Destroy(gameObject);
        }
    }

    // NPC�� ���� Instantiate �ϴ� �޼���.
    // hasSpawned üũ�� �� ���� ����.
    public void SpawnWaveNPC()
    {
        if (hasSpawned) return;

        // npcPrefabs�� spawnPoints�� ��Ī�� ������ŭ ��ȸ
        for (int i = 0; i < npcPrefabs.Length && i < spawnPoints.Length; i++)
        {
            Instantiate(
                npcPrefabs[i],
                spawnPoints[i].position,
                Quaternion.identity);
        }

        hasSpawned = true;

        // ���� Ÿ�̸� ����
        if (!disableSpawnTimer && SpawnTimer.Instance != null)
            SpawnTimer.Instance.StartCountdown(60);

        // �ִϸ����� Ʈ����(���� ���� �ִϸ��̼ǿ�)
        if (animator != null)
            animator.SetTrigger(SpawnReleasedHash);
    }

    // �����ʸ� ��Ȱ���ϰų� �����ϱ� ���� ȣ��.
    // ���� ����(�÷���)�� �ʱ�ȭ.
    public void Reset()
    {
        hasSpawned = false;
        PlayerTouched = false;

        // Animator�� �پ� ���� ���� Ʈ���� ����
        if (animator != null)
            animator.ResetTrigger(SpawnReleasedHash);
    }
}
