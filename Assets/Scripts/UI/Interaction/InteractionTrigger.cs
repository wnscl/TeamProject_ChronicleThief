using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractionTrigger : MonoBehaviour
    {
        public MonoBehaviour interactable;  // ��ȣ�ۿ� ��� (Inspector���� ����)

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            // �÷��̾ ������ InteractionSystem�� ���
            InteractionSystem.Instance.SetActiveTrigger(this);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            // �÷��̾ ������ ��� ����
            InteractionSystem.Instance.ClearActiveTrigger(this);
        }

        // ��ȣ�ۿ� ����
        public void DoInteract()
        {
            if (interactable == null) return;
            if (interactable is IInteractable interactableObj)
                interactableObj.Interact(GameObject.FindWithTag("Player"));
        }
    }
}