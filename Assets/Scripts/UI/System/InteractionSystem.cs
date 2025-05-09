using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Instance { get; private set; }

    private InteractionTrigger activeTrigger;  // 현재 플레이어 발밑에 있는 트리거

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        // E 키 누르면, 등록된 트리거의 DoInteract 호출
        if (activeTrigger != null && Input.GetKeyDown(KeyCode.E))
        {
            activeTrigger.DoInteract();
        }
    }

    // InteractionTrigger가 플레이어 진입 시 호출
    public void SetActiveTrigger(InteractionTrigger trigger)
    {
        activeTrigger = trigger;
    }

    // 플레이어 벗어날 때 호출
    public void ClearActiveTrigger(InteractionTrigger trigger)
    {
        if (activeTrigger == trigger)
            activeTrigger = null;
    }
}
 