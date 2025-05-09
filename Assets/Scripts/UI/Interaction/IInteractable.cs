using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        // interactor = 상호작용을 시도한 오브젝트, 해당 오브젝트와 상호작용 실행.
        void Interact(GameObject interactor);
    }
}