using UnityEngine;
using NPC;
using UI;

// 10웨이브 종료 후 소환되는 메인NPC의 기능 스크립트.
// ‘Use’ 버튼을 눌렀을 때 현재 웨이브가 19이고 Ready 상태라면
// 추후 구현할 기능(지금은 빈 메서드)을 호출하고,
// 그렇지 않으면 Use/Skip 각각 다른 대사를 출력.
public class Wave10MainNPCFunction : MonoBehaviour, INPCFunction
{
    private NPCController npcController;

    void Awake()
    {
        npcController = GetComponent<NPCController>();
    }

    public void Execute(GameObject interactor)
    {
        // 1) 대화창에 첫 번째 대사만 띄우고, 선택창 노출
        string speaker = npcController.npcName;
        string line = npcController.dialogueLines[0];
        UIManager.Instance.ShowChoice(
            speaker,
            line,

            // “Use” 선택 시
            () =>
            {
                //// 2) 조건 검사: 현재 웨이브가 19이고 Ready 단계인지
                //bool isReadyForUpgrade =
                //    BattleSystemManager.Instance.CurrentWave == 19 &&
                //    BattleSystemManager.Instance.IsInReady;

                //if (isReadyForUpgrade)
                //{
                //    // TODO: 실제 기능 구현 자리
                //    PerformSpecialFunction(interactor);
                //}
                //else
                //{
                //    // 준비가 안 됐을 때 대사
                //    UIManager.Instance.ShowDialog(
                //        speaker,
                //        "아직 강화할 준비가 되지 않았습니다.");
                //}
            },

            // “Skip” 선택 시
            () =>
            {
                UIManager.Instance.ShowDialog(
                    speaker,
                    "스킵 대사.");
            }
        );
    }

    private void PerformSpecialFunction(GameObject interactor)
    {
        Debug.Log("Wave10MainNPC: PerformSpecialFunction 호출됨");
    }
}