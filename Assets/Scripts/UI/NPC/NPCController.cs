using UnityEngine;
using Interaction;
using UI;
using System.Collections;

namespace NPC
{
    public class NPCController : MonoBehaviour, IInteractable
    {
        public string npcName;             // NPC �̸�
        public string[] dialogueLines;     // ��� ���
        [SerializeField] private float activeTime = 60f; // Ȱ�� �ð� (�⺻ 1��)
        private bool isActive = true;
        private INPCFunction npcFunction;  // NPC ���� ���

        void Awake()
        {
            // NPC�� ���� ��� ������Ʈ ��������
            npcFunction = GetComponent<INPCFunction>();
        }

        void Start()
        {
            // Ÿ�̸� ����
            StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(activeTime);
            isActive = false;
            UIManager.Instance.HideDialog();
            Destroy(gameObject); // NPC ����
        }

        // �÷��̾�� ��ȣ�ۿ� �� ȣ��
        public void Interact(GameObject interactor)
        {
            if (!isActive) return;
            UIManager.Instance.ShowDialog(npcName, dialogueLines[0]);
            npcFunction?.Execute(interactor);
        }
    }
}