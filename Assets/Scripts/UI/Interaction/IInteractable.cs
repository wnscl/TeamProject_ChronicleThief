using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        // interactor = ��ȣ�ۿ��� �õ��� ������Ʈ, �ش� ������Ʈ�� ��ȣ�ۿ� ����.
        void Interact(GameObject interactor);
    }
}