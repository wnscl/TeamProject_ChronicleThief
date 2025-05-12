using NPC;
using UI;
using UnityEngine;

public class UpgradeNPCFunction : MonoBehaviour, INPCFunction
{
    [SerializeField] private string[] dialogueLines; // [0] ���� ��û ����

    public void Execute(GameObject interactor)
    {
        // 1) ù ���
        UIManager.Instance.ShowDialog(
            GetComponent<NPCController>().npcName,
            dialogueLines[0]);
        // 2) ���׷��̵� UI ����
        UpgradeUI.Instance.Show(
            GetComponent<NPCController>().npcName);
        // (3) ��ȭâ �ϴܿ� ����ŵ���� �����,
        //      ������ ���׷��̵� �гΰ� ��ȭâ�� �ݵ��� ����
        UIManager.Instance.ShowSkipOnly(() => {
            UpgradeUI.Instance.HidePanel();
            UIManager.Instance.HideDialog();
        });
    }
}