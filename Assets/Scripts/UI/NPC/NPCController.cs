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
        private bool isActive = true;
        private INPCFunction npcFunction;  // NPC 고유 기능

        void Awake()
        {
            // NPC에 붙은 기능 컴포넌트 가져오기
            npcFunction = GetComponent<INPCFunction>();
        }

        void Start()
        {
            // 타이머 시작
            StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(activeTime);
            isActive = false;
            UIManager.Instance.HideDialog();
            Destroy(gameObject); // NPC 제거
        }

        // 플레이어와 상호작용 시 호출
        public void Interact(GameObject interactor)
        {
            if (!isActive) return;
            // 첫 번째 대사 & 선택창 띄우기
            UIManager.Instance.ShowChoice(
              npcName,
              dialogueLines[0],
              // “특수 기능 사용” 선택 시
              () => {
                  npcFunction?.Execute(interactor);
                  UIManager.Instance.ShowDialog(npcName, dialogueLines[1]);
              },
              // “건너뛰기” 선택 시
              () => {
                  UIManager.Instance.ShowDialog(npcName, dialogueLines[2]);
              }
            );
        }
    }
}