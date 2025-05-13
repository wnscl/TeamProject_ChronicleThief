using UnityEngine;
using Interaction;
using UI;
using System.Collections;

namespace NPC
{
    public class NPCController : MonoBehaviour, IInteractable
    {
        public string npcName;             // NPC 이름
        public string[] dialogueLines;     // 대사 목록
        [SerializeField] private float activeTime = 60f; // 활성 시간 (기본 1분)
        [SerializeField] private float despawnAnimationDuration = 0.3f; // 애니메이션 재생 시간
        private bool isActive = true;

        private INPCFunction npcFunction;  // NPC 고유 기능
        private Animator animator;

        void Awake()
        {
            npcFunction = GetComponent<INPCFunction>(); // NPC에 붙은 기능 컴포넌트 가져오기
            animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        }

        void Start()
        {
            // 타이머 시작
            StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            // 1) 활성 시간 대기
            yield return new WaitForSeconds(activeTime);
            isActive = false;

            // 2) 대화창 숨기기
            UIManager.Instance.HideDialog();

            // 3) 강화, 회복 패널 숨기기
            if (UpgradeUI.Instance != null)
                UpgradeUI.Instance.Hide();
            if (HealUI.Instance != null)
                HealUI.Instance.Hide();

            // 4) 스폰 해제 애니메이터 트리거
            if (animator != null)
                animator.SetTrigger("SpawnReleased");

            // 5) 애니메이션 재생 시간만큼 대기
            yield return new WaitForSeconds(despawnAnimationDuration);

            // 6) 오브젝트 제거
            Destroy(gameObject);
        }

        // 플레이어와 상호작용 시 호출
        public void Interact(GameObject interactor)
        {
            if (!isActive) return;

            UIManager.Instance.ShowChoice(
                npcName,
                dialogueLines[0],
                // “특수 기능 사용” 선택 시
                () =>
                {
                    npcFunction?.Execute(interactor);
                    UIManager.Instance.ShowDialog(npcName, dialogueLines[1]);
                },
                // “건너뛰기” 선택 시
                () =>
                {
                    // 1) 대사 보여주고
                    UIManager.Instance.ShowDialog(npcName, dialogueLines[2]);
                    // 2) 1초 뒤 숨기기 시작
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