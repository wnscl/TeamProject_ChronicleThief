using NPC;
using UI;
using UnityEngine;

public class UpgradeNPCFunction : MonoBehaviour, INPCFunction
{
    [SerializeField] private string[] dialogueLines; // [0] 선택 요청 문구

    public void Execute(GameObject interactor)
    {
        // 1) 첫 대사
        UIManager.Instance.ShowDialog(
            GetComponent<NPCController>().npcName,
            dialogueLines[0]);
        // 2) 업그레이드 UI 오픈
        UpgradeUI.Instance.Show(
            GetComponent<NPCController>().npcName);
        // (3) 대화창 하단에 “스킵”만 띄워서,
        //      누르면 업그레이드 패널과 대화창을 닫도록 연결
        UIManager.Instance.ShowSkipOnly(() => {
            UpgradeUI.Instance.HidePanel();
            UIManager.Instance.HideDialog();
        });
    }
}