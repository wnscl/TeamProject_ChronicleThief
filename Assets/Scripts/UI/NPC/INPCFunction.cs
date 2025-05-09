using UnityEngine;

namespace NPC
{
    public interface INPCFunction
    {
        // interactor = 상호작용 주체 (플레이어)가 NPC의 고유 기능을 실행
        void Execute(GameObject interactor);
    }
}