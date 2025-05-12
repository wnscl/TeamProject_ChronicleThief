using UnityEngine;
using NPC;
using UI;

public class HealNPCFunction : MonoBehaviour, INPCFunction
{
    // NPC�� ���� �� ���� �ʱ�ȭ�ϱ� ���� �÷���
    private bool initialized = false;

    // Inspector���� ���� ������ ù ���
    [Tooltip("NPC�� ��ȣ�ۿ� �� ó�� ������ ���")]
    public string initialDialogue = "ü���� �ʿ��ϽŰ���?";

    // ��ȣ�ۿ� �� ȣ��Ǵ� �޼���
    public void Execute(GameObject interactor)
    {
        // ù ���� �ÿ��� ȸ�� ��� �ʱ�ȭ
        if (!initialized)
        {
            HealUI.Instance.ResetHeals();
            initialized = true;
        }

        // NPC �̸��� ��� ǥ��
        var speaker = GetComponent<NPCController>().npcName;
        UIManager.Instance.ShowDialog(speaker, initialDialogue);

        // �� UI ����
        HealUI.Instance.Show();

        // ��ŵ ��ư�� �����, ������ UI�� ��ȭâ �ݱ�
        UIManager.Instance.ShowSkipOnly(() =>
        {
            HealUI.Instance.Hide();
            UIManager.Instance.HideDialog();
        });
    }
}