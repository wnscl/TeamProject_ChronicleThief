using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Instance { get; private set; }

    private InteractionTrigger activeTrigger;  // ���� �÷��̾� �߹ؿ� �ִ� Ʈ����

    void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        // E Ű ������, ��ϵ� Ʈ������ DoInteract ȣ��
        if (activeTrigger != null && Input.GetKeyDown(KeyCode.E))
        {
            activeTrigger.DoInteract();
        }
    }

    // InteractionTrigger�� �÷��̾� ���� �� ȣ��
    public void SetActiveTrigger(InteractionTrigger trigger)
    {
        activeTrigger = trigger;
    }

    // �÷��̾� ��� �� ȣ��
    public void ClearActiveTrigger(InteractionTrigger trigger)
    {
        if (activeTrigger == trigger)
            activeTrigger = null;
    }
}
 