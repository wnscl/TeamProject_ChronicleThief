// Assets/Scripts/UI/UpgradeNPCFunction.cs

using UnityEngine;
using NPC;
using UI;

public class UpgradeNPCFunction : MonoBehaviour, INPCFunction
{
    // 이 NPC와 상호작용할 때 한 번만 강화 기록을 초기화하기 위한 플래그
    private bool initialized = false;

    // INPCFunction.Execute가 호출될 때마다 실행됩니다.
    public void Execute(GameObject interactor)
    {
        // 최초 호출 시에만 플레이어 강화 기록 초기화
        if (!initialized)
        {
            var mgr = interactor.GetComponent<PlayerUpgradeManager>();
            if (mgr != null) mgr.ResetUpgrades();
            initialized = true;
        }

        // 강화 UI 표시 (NPCController에서 이미 첫 대사는 출력됨)
        UpgradeUI.Instance.Show();

        // '스킵' 버튼만 남기고 누르면 강화 패널과 대화창 모두 닫기
        UIManager.Instance.ShowSkipOnly(() =>
        {
            UpgradeUI.Instance.Hide();
            UIManager.Instance.HideDialog();
        });
    }
}