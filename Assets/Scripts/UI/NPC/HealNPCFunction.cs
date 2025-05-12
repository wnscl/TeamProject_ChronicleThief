using UnityEngine;
using NPC;
using UI;

public class HealNPCFunction : MonoBehaviour, INPCFunction
{
    // NPC당 최초 한 번만 초기화하기 위한 플래그
    private bool initialized = false;

    // Inspector에서 설정 가능한 첫 대사
    [Tooltip("NPC와 상호작용 시 처음 보여줄 대사")]
    public string initialDialogue = "체력이 필요하신가요?";

    // 상호작용 시 호출되는 메서드
    public void Execute(GameObject interactor)
    {
        // 첫 실행 시에만 회복 기록 초기화
        if (!initialized)
        {
            HealUI.Instance.ResetHeals();
            initialized = true;
        }

        // NPC 이름과 대사 표시
        var speaker = GetComponent<NPCController>().npcName;
        UIManager.Instance.ShowDialog(speaker, initialDialogue);

        // 힐 UI 열기
        HealUI.Instance.Show();

        // 스킵 버튼만 남기고, 누르면 UI와 대화창 닫기
        UIManager.Instance.ShowSkipOnly(() =>
        {
            HealUI.Instance.Hide();
            UIManager.Instance.HideDialog();
        });
    }
}