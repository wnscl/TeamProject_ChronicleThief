using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractionTrigger : MonoBehaviour
    {
        public MonoBehaviour interactable;  // 상호작용 대상 (Inspector에서 설정)

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            // 플레이어가 들어오면 InteractionSystem에 등록
            InteractionSystem.Instance.SetActiveTrigger(this);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            // 플레이어가 나가면 등록 해제
            InteractionSystem.Instance.ClearActiveTrigger(this);
        }

        // 상호작용 실행
        public void DoInteract()
        {
            if (interactable == null) return;
            if (interactable is IInteractable interactableObj)
                interactableObj.Interact(GameObject.FindWithTag("Player"));
        }
    }
}