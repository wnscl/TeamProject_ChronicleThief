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
        [SerializeField] private float despawnAnimationDuration = 0.3f; // �ִϸ��̼� ��� �ð�
        private bool isActive = true;

        private INPCFunction npcFunction;  // NPC ���� ���
        private Animator animator;

        void Awake()
        {
            npcFunction = GetComponent<INPCFunction>(); // NPC�� ���� ��� ������Ʈ ��������
            animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
        }

        void Start()
        {
            // Ÿ�̸� ����
            StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            // 1) Ȱ�� �ð� ���
            yield return new WaitForSeconds(activeTime);
            isActive = false;

            // 2) ��ȭâ �����
            UIManager.Instance.HideDialog();

            // 3) ��ȭ, ȸ�� �г� �����
            if (UpgradeUI.Instance != null)
                UpgradeUI.Instance.Hide();
            if (HealUI.Instance != null)
                HealUI.Instance.Hide();

            // 4) ���� ���� �ִϸ����� Ʈ����
            if (animator != null)
                animator.SetTrigger("SpawnReleased");

            // 5) �ִϸ��̼� ��� �ð���ŭ ���
            yield return new WaitForSeconds(despawnAnimationDuration);

            // 6) ������Ʈ ����
            Destroy(gameObject);
        }

        // �÷��̾�� ��ȣ�ۿ� �� ȣ��
        public void Interact(GameObject interactor)
        {
            if (!isActive) return;

            UIManager.Instance.ShowChoice(
                npcName,
                dialogueLines[0],
                // ��Ư�� ��� ��롱 ���� ��
                () =>
                {
                    npcFunction?.Execute(interactor);
                    UIManager.Instance.ShowDialog(npcName, dialogueLines[1]);
                },
                // ���ǳʶٱ⡱ ���� ��
                () =>
                {
                    // 1) ��� �����ְ�
                    UIManager.Instance.ShowDialog(npcName, dialogueLines[2]);
                    // 2) 1�� �� ����� ����
                    StartCoroutine(HideDialogAfterDelay(1f));
                }
            );
        }
        private IEnumerator HideDialogAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UIManager.Instance.HideDialog();
        }
    }
}