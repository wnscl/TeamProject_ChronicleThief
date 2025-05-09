using UnityEngine;

namespace Interaction
{
    public class InteractionSystem : MonoBehaviour
    {
        public static InteractionSystem Instance { get; private set; }

        private InteractionTrigger activeTrigger;  // ���� Ȱ��ȭ�� Ʈ����

        void Awake()
        {
            // �̱��� ����
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        void Update()
        {
            // E Ű �Է� �� ��ȣ�ۿ� ����
            if (activeTrigger != null && Input.GetKeyDown(KeyCode.E))
            {   
                activeTrigger.DoInteract();
            }
        }

        // Ʈ���Ű� Ȱ��ȭ�� �� ȣ��
        public void SetActiveTrigger(InteractionTrigger trigger)
        {
            activeTrigger = trigger;
        }

        // Ʈ���Ű� ��Ȱ��ȭ�� �� ȣ��
        public void ClearActiveTrigger(InteractionTrigger trigger)
        {
            if (activeTrigger == trigger)
                activeTrigger = null;
        }
    }
}