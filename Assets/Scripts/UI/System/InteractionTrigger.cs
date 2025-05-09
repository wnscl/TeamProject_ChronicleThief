using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour
{
    // Inspector에서 드래그 앤 드롭
    public MonoBehaviour interactable;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // 플레이어가 범위에 들어오면 InteractionSystem에 등록
        InteractionSystem.Instance.SetActiveTrigger(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // 벗어나면 해제
        InteractionSystem.Instance.ClearActiveTrigger(this);
    }

    // E키 눌렀을 때 실행되는 실제 상호작용
    public void DoInteract()
    {
        if (interactable == null) return;

        // 타입에 따라 분기 처리
        if (interactable is NPCController npc)
            npc.OnInteract();
    }
}
