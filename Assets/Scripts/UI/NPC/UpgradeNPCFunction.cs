// Assets/Scripts/UI/UpgradeNPCFunction.cs

using UnityEngine;
using NPC;
using UI;

public class UpgradeNPCFunction : MonoBehaviour, INPCFunction
{
    // �� NPC�� ��ȣ�ۿ��� �� �� ���� ��ȭ ����� �ʱ�ȭ�ϱ� ���� �÷���
    private bool initialized = false;

    // INPCFunction.Execute�� ȣ��� ������ ����˴ϴ�.
    public void Execute(GameObject interactor)
    {
        // ���� ȣ�� �ÿ��� �÷��̾� ��ȭ ��� �ʱ�ȭ
        if (!initialized)
        {
            var mgr = interactor.GetComponent<PlayerUpgradeManager>();
            if (mgr != null) mgr.ResetUpgrades();
            initialized = true;
        }

        // ��ȭ UI ǥ�� (NPCController���� �̹� ù ���� ��µ�)
        UpgradeUI.Instance.Show();

        // '��ŵ' ��ư�� ����� ������ ��ȭ �гΰ� ��ȭâ ��� �ݱ�
        UIManager.Instance.ShowSkipOnly(() =>
        {
            UpgradeUI.Instance.Hide();
            UIManager.Instance.HideDialog();
        });
    }
}