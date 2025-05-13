using UnityEngine;
using NPC;
using UI;

// 20���̺�(���� ���� Ŭ����) �� ��ȯ�Ǵ� ����NPC�� ��� ��ũ��Ʈ.
// �ܼ��� ��ȭ�� ����, Use/Skip ��� ���� �������� ó��.
public class FinalMainNPCFunction : MonoBehaviour, INPCFunction
{
    private NPCController npcController;

    void Awake()
    {
        npcController = GetComponent<NPCController>();
    }

    public void Execute(GameObject interactor)
    {
        // ��� + �� ��ư ����
        string speaker = npcController.npcName;
        string line = npcController.dialogueLines[0];
        UIManager.Instance.ShowChoice(
            speaker,
            line,

            // ��Use�� ���� ��
            () =>
            {
                UIManager.Instance.ShowDialog(
                    speaker,
                    "use ���");
            },

            // ��Skip�� ���� ��
            () =>
            {
                UIManager.Instance.ShowDialog(
                    speaker,
                    "skip ���");
            }
        );
    }
}