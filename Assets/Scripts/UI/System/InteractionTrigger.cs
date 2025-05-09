using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour
{
    // Inspector���� �巡�� �� ���
    public MonoBehaviour interactable;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // �÷��̾ ������ ������ InteractionSystem�� ���
        InteractionSystem.Instance.SetActiveTrigger(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // ����� ����
        InteractionSystem.Instance.ClearActiveTrigger(this);
    }

    // EŰ ������ �� ����Ǵ� ���� ��ȣ�ۿ�
    public void DoInteract()
    {
        if (interactable == null) return;

        // Ÿ�Կ� ���� �б� ó��
        if (interactable is NPCController npc)
            npc.OnInteract();
    }
}
