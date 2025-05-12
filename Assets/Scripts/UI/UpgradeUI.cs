using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    [Header("패널")]
    public GameObject panel;       // 강화 UI 전체 패널
    public TMP_Text goldText;    // 남은 골드 표시

    [Header("강화 버튼")]
    public Button attackBtn;
    public Button defenseBtn;
    public Button healthBtn;
    public Button speedBtn;

    [Header("비용 텍스트")]
    public TMP_Text attackCostText;
    public TMP_Text defenseCostText;
    public TMP_Text healthCostText;
    public TMP_Text speedCostText;

    [Header("참조 컴포넌트")]
    public PlayerUpgradeManager upgradeManager;
    public ResourcesHandler resourcesHandler;

    private enum StatType { Attack, Defense, Health, Speed }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        panel.SetActive(false);

        if (resourcesHandler == null)
            resourcesHandler = ResourcesHandler.Instance;

        // 버튼 클릭 시 항상 TryUpgrade만 호출 (Show에서 상태 결정)
        attackBtn.onClick.AddListener(() => TryUpgrade(StatType.Attack));
        defenseBtn.onClick.AddListener(() => TryUpgrade(StatType.Defense));
        healthBtn.onClick.AddListener(() => TryUpgrade(StatType.Health));
        speedBtn.onClick.AddListener(() => TryUpgrade(StatType.Speed));

        UpdateAllTexts();
    }

    // 강화 UI 열기
    public void Show()
    {
        panel.SetActive(true);

        // 닫았다 열어도, 내부 count를 그대로 사용해서 인터랙티블 설정
        attackBtn.interactable = upgradeManager.GetAttackCount() < upgradeManager.MaxUpgradesPerStat;
        defenseBtn.interactable = upgradeManager.GetDefenseCount() < upgradeManager.MaxUpgradesPerStat;
        healthBtn.interactable = upgradeManager.GetHealthCount() < upgradeManager.MaxUpgradesPerStat;
        speedBtn.interactable = upgradeManager.GetSpeedCount() < upgradeManager.MaxUpgradesPerStat;

        UpdateAllTexts();
    }

    // 강화 시도
    private void TryUpgrade(StatType stat)
    {
        int cost = 0;
        bool isMax = false;

        // 비용·최대치 여부 조회
        switch (stat)
        {
            case StatType.Attack:
                cost = upgradeManager.GetAttackCost();
                isMax = upgradeManager.GetAttackCount() >= upgradeManager.MaxUpgradesPerStat;
                break;
            case StatType.Defense:
                cost = upgradeManager.GetDefenseCost();
                isMax = upgradeManager.GetDefenseCount() >= upgradeManager.MaxUpgradesPerStat;
                break;
            case StatType.Health:
                cost = upgradeManager.GetHealthCost();
                isMax = upgradeManager.GetHealthCount() >= upgradeManager.MaxUpgradesPerStat;
                break;
            case StatType.Speed:
                cost = upgradeManager.GetSpeedCost();
                isMax = upgradeManager.GetSpeedCount() >= upgradeManager.MaxUpgradesPerStat;
                break;
        }

        if (isMax)
        {
            UIManager.Instance.ShowDialog("강화사", "이미 최대 강화치입니다!");
            return;
        }

        if (!resourcesHandler.SpendGold(cost))
        {
            UIManager.Instance.ShowDialog("강화사", "골드가 부족합니다!");
            return;
        }

        bool success = false;
        switch (stat)
        {
            case StatType.Attack: success = upgradeManager.TryUpgradeAttack(); break;
            case StatType.Defense: success = upgradeManager.TryUpgradeDefense(); break;
            case StatType.Health: success = upgradeManager.TryUpgradeHealth(); break;
            case StatType.Speed: success = upgradeManager.TryUpgradeSpeed(); break;
        }

        UIManager.Instance.ShowDialog("강화사", success ? "강화 성공!" : "강화 실패");

        // 강화 성공 여부와 관계없이, 버튼 상태는 Show() 로 다시 결정
        UpdateAllTexts();
    }

    // UI 텍스트 업데이트
    private void UpdateAllTexts()
    {
        attackCostText.text = $"공격력: {(upgradeManager.GetAttackCount() >= upgradeManager.MaxUpgradesPerStat ? "완료" : upgradeManager.GetAttackCost() + "G")}";
        defenseCostText.text = $"방어력: {(upgradeManager.GetDefenseCount() >= upgradeManager.MaxUpgradesPerStat ? "완료" : upgradeManager.GetDefenseCost() + "G")}";
        healthCostText.text = $"체력: {(upgradeManager.GetHealthCount() >= upgradeManager.MaxUpgradesPerStat ? "완료" : upgradeManager.GetHealthCost() + "G")}";
        speedCostText.text = $"이동속도: {(upgradeManager.GetSpeedCount() >= upgradeManager.MaxUpgradesPerStat ? "완료" : upgradeManager.GetSpeedCost() + "G")}";

        goldText.text = $"Gold: {resourcesHandler.Gold}G";
    }

    // 강화 UI 닫기
    public void Hide()
    {
        panel.SetActive(false);
    }
}
