// Assets/Scripts/UI/HealNPCFunction.cs

using UnityEngine;
using NPC;
using UI;

public class HealNPCFunction : MonoBehaviour, INPCFunction
{
    // �� NPC�� ��ȣ�ۿ��� �� �� ���� ȸ�� ����� �ʱ�ȭ�ϱ� ���� �÷���
    private bool initialized = false;

    // INPCFunction.Execute�� ȣ��� ������ ����˴ϴ�.
    public void Execute(GameObject interactor)
    {
        // ���� ȣ�� �ÿ��� �� UI ��� �̷� �ʱ�ȭ
        if (!initialized)
        {
            HealUI.Instance.ResetHeals();
            initialized = true;
        }

        // �� UI ǥ�� (NPCController���� �̹� ù ���� ��µ�)
        HealUI.Instance.Show();

        // '��ŵ' ��ư�� ����� ������ �� �гΰ� ��ȭâ ��� �ݱ�
        UIManager.Instance.ShowSkipOnly(() =>
        {
            HealUI.Instance.Hide();
            UIManager.Instance.HideDialog();
        });
    }
}