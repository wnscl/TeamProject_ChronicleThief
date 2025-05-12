using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    [Header("패널")]
    public GameObject panel;

    [Header("버튼")]
    public Button attackButton;
    public Button defenseButton;
    public Button healthButton;
    public Button speedButton;

    [Header("버튼 내 코스트 텍스트")]
    public TMP_Text attackCostText;
    public TMP_Text defenseCostText;
    public TMP_Text healthCostText;
    public TMP_Text speedCostText;

    [Header("골드 텍스트")]
    public TMP_Text goldText;

    private PlayerUpgradeManager playerStats;
    private string npcName;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        panel.SetActive(false);

        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            playerStats = playerObj.GetComponent<PlayerUpgradeManager>();
        else
            Debug.LogError("Player object with PlayerUpgradeManager not found");

        attackButton.onClick.AddListener(() => OnUpgrade(
            playerStats.TryUpgradeAttack(),
            playerStats.GetAttackCost(), "공격력"));
        defenseButton.onClick.AddListener(() => OnUpgrade(
            playerStats.TryUpgradeDefense(),
            playerStats.GetDefenseCost(), "방어력"));
        healthButton.onClick.AddListener(() => OnUpgrade(
            playerStats.TryUpgradeHealth(),
            playerStats.GetHealthCost(), "체력"));
        speedButton.onClick.AddListener(() => OnUpgrade(
            playerStats.TryUpgradeSpeed(),
            playerStats.GetSpeedCost(), "이동속도"));
    }

    public void Show(string speaker)
    {
        npcName = speaker;
        UpdateCostTexts();
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }

    void OnUpgrade(bool success, int nextCost, string statName)
    {
        bool isMaxed = false;
        // 각 스탯별 강화 횟수 체크
        switch (statName)
        {
            case "공격력":
                isMaxed = playerStats.GetAttackCount() >= playerStats.MaxUpgradesPerStat;
                break;
            case "방어력":
                isMaxed = playerStats.GetDefenseCount() >= playerStats.MaxUpgradesPerStat;
                break;
            case "체력":
                isMaxed = playerStats.GetHealthCount() >= playerStats.MaxUpgradesPerStat;
                break;
            case "이동속도":
                isMaxed = playerStats.GetSpeedCount() >= playerStats.MaxUpgradesPerStat;
                break;
        }
        if (success)
        {
            if (isMaxed)
                UIManager.Instance.ShowDialog(npcName,
                        $"{statName} 강화 완료!");
            else
                UIManager.Instance.ShowDialog(npcName,
                    $"{statName} 강화 성공! 다음 코스트: {nextCost} 골드");
        }
        else
            UIManager.Instance.ShowDialog(npcName,
                $"강화 실패! 골드가 부족하거나 모든 강화를 완료했습니다!");

        UpdateCostTexts();
    }

    void UpdateCostTexts()
    {
        if (playerStats == null) return;

        goldText.text = $"Gold: {playerStats.GetCurrentGold()}";

        if (playerStats.GetAttackCount() >= playerStats.MaxUpgradesPerStat)
            attackCostText.text = "강화 완료";
        else
            attackCostText.text = $"공격력: {playerStats.GetAttackCost()} G";

        // 방어력
        if (playerStats.GetDefenseCount() >= playerStats.MaxUpgradesPerStat)
            defenseCostText.text = "강화 완료";
        else
            defenseCostText.text = $"방어력: {playerStats.GetDefenseCost()} G";

        // 체력
        if (playerStats.GetHealthCount() >= playerStats.MaxUpgradesPerStat)
            healthCostText.text = "강화 완료";
        else
            healthCostText.text = $"체력: {playerStats.GetHealthCost()} G";

        // 이동속도
        if (playerStats.GetSpeedCount() >= playerStats.MaxUpgradesPerStat)
            speedCostText.text = "강화 완료";
        else
            speedCostText.text = $"이동속도: {playerStats.GetSpeedCost()} G";
    }

}
