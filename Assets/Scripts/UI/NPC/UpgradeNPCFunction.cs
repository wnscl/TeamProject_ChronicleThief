using UnityEngine;
using NPC;
using UI;

public class UpgradeNPCFunction : MonoBehaviour, INPCFunction
{
    // NPC�� ���� �� ���� �ʱ�ȭ�ϱ� ���� �÷���
    private bool initialized = false;

    // Inspector���� ���� ������ ù ���
    [Tooltip("NPC�� ��ȣ�ۿ� �� ó�� ������ ���")]
    public string initialDialogue = "��ȭ�� �ʿ��ϽŰ���?";

    // ��ȣ�ۿ� �� ȣ��Ǵ� �޼���
    public void Execute(GameObject interactor)
    {
        // ù ���� �ÿ��� �÷��̾��� ��ȭ ��� �ʱ�ȭ
        if (!initialized)
        {
            var mgr = interactor.GetComponent<PlayerUpgradeManager>();
            if (mgr != null) mgr.ResetUpgrades();
            initialized = true;
        }

        // NPC �̸��� ��� ǥ��
        var speaker = GetComponent<NPCController>().npcName;
        UIManager.Instance.ShowDialog(speaker, initialDialogue);

        // ��ȭ UI ����
        UpgradeUI.Instance.Show();

        // ��ŵ ��ư�� �����, ������ UI�� ��ȭâ �ݱ�
        UIManager.Instance.ShowSkipOnly(() =>
        {
            UpgradeUI.Instance.Hide();
            UIManager.Instance.HideDialog();
        });
    }
}