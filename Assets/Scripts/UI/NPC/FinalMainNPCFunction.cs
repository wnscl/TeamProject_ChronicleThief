using UnityEngine;
using NPC;
using UI;

// 20웨이브(최종 보스 클리어) 후 소환되는 메인NPC의 기능 스크립트.
// 단순히 대화를 띄우고, Use/Skip 모두 같은 로직으로 처리.
public class FinalMainNPCFunction : MonoBehaviour, INPCFunction
{
    private NPCController npcController;

    void Awake()
    {
        npcController = GetComponent<NPCController>();
    }

    public void Execute(GameObject interactor)
    {
        // 대사 + 두 버튼 노출
        string speaker = npcController.npcName;
        string line = npcController.dialogueLines[0];
        UIManager.Instance.ShowChoice(
            speaker,
            line,

            // “Use” 선택 시
            () =>
            {
                UIManager.Instance.ShowDialog(
                    speaker,
                    "use 대사");
            },

            // “Skip” 선택 시
            () =>
            {
                UIManager.Instance.ShowDialog(
                    speaker,
                    "skip 대사");
            }
        );
    }
}