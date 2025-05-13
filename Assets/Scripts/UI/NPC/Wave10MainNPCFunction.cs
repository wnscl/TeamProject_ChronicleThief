using UnityEngine;
using NPC;
using UI;

// 10���̺� ���� �� ��ȯ�Ǵ� ����NPC�� ��� ��ũ��Ʈ.
// ��Use�� ��ư�� ������ �� ���� ���̺갡 19�̰� Ready ���¶��
// ���� ������ ���(������ �� �޼���)�� ȣ���ϰ�,
// �׷��� ������ Use/Skip ���� �ٸ� ��縦 ���.
public class Wave10MainNPCFunction : MonoBehaviour, INPCFunction
{
    private NPCController npcController;

    void Awake()
    {
        npcController = GetComponent<NPCController>();
    }

    public void Execute(GameObject interactor)
    {
        // 1) ��ȭâ�� ù ��° ��縸 ����, ����â ����
        string speaker = npcController.npcName;
        string line = npcController.dialogueLines[0];
        UIManager.Instance.ShowChoice(
            speaker,
            line,

            // ��Use�� ���� ��
            () =>
            {
                //// 2) ���� �˻�: ���� ���̺갡 19�̰� Ready �ܰ�����
                //bool isReadyForUpgrade =
                //    BattleSystemManager.Instance.CurrentWave == 19 &&
                //    BattleSystemManager.Instance.IsInReady;

                //if (isReadyForUpgrade)
                //{
                //    // TODO: ���� ��� ���� �ڸ�
                //    PerformSpecialFunction(interactor);
                //}
                //else
                //{
                //    // �غ� �� ���� �� ���
                //    UIManager.Instance.ShowDialog(
                //        speaker,
                //        "���� ��ȭ�� �غ� ���� �ʾҽ��ϴ�.");
                //}
            },

            // ��Skip�� ���� ��
            () =>
            {
                UIManager.Instance.ShowDialog(
                    speaker,
                    "��ŵ ���.");
            }
        );
    }

    private void PerformSpecialFunction(GameObject interactor)
    {
        Debug.Log("Wave10MainNPC: PerformSpecialFunction ȣ���");
    }
}