using UnityEngine;

namespace Interaction
{
    public class InteractionSystem : MonoBehaviour
    {
        public static InteractionSystem Instance { get; private set; }

        private InteractionTrigger activeTrigger;  // 현재 활성화된 트리거

        void Awake()
        {
            // 싱글톤 패턴
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        void Update()
        {
            // E 키 입력 시 상호작용 실행
            if (activeTrigger != null && Input.GetKeyDown(KeyCode.E))
            {   
                activeTrigger.DoInteract();
            }
        }

        // 트리거가 활성화될 때 호출
        public void SetActiveTrigger(InteractionTrigger trigger)
        {
            activeTrigger = trigger;
        }

        // 트리거가 비활성화될 때 호출
        public void ClearActiveTrigger(InteractionTrigger trigger)
        {
            if (activeTrigger == trigger)
                activeTrigger = null;
        }
    }
}