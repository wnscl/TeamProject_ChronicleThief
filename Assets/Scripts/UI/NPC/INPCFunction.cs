using UnityEngine;

namespace NPC
{
    public interface INPCFunction
    {
        // interactor = ��ȣ�ۿ� ��ü (�÷��̾�)�� NPC�� ���� ����� ����
        void Execute(GameObject interactor);
    }
}