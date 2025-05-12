using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    [Header("�г�")]
    public GameObject panel;

    [Header("��ư")]
    public Button attackButton;
    public Button defenseButton;
    public Button healthButton;
    public Button speedButton;

    [Header("��ư �� �ڽ�Ʈ �ؽ�Ʈ")]
    public TMP_Text attackCostText;
    public TMP_Text defenseCostText;
    public TMP_Text healthCostText;
    public TMP_Text speedCostText;

    [Header("��� �ؽ�Ʈ")]
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
            playerStats.GetAttackCost(), "���ݷ�"));
        defenseButton.onClick.AddListener(() => OnUpgrade(
            playerStats.TryUpgradeDefense(),
            playerStats.GetDefenseCost(), "����"));
        healthButton.onClick.AddListener(() => OnUpgrade(
            playerStats.TryUpgradeHealth(),
            playerStats.GetHealthCost(), "ü��"));
        speedButton.onClick.AddListener(() => OnUpgrade(
            playerStats.TryUpgradeSpeed(),
            playerStats.GetSpeedCost(), "�̵��ӵ�"));
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
        // �� ���Ⱥ� ��ȭ Ƚ�� üũ
        switch (statName)
        {
            case "���ݷ�":
                isMaxed = playerStats.GetAttackCount() >= playerStats.MaxUpgradesPerStat;
                break;
            case "����":
                isMaxed = playerStats.GetDefenseCount() >= playerStats.MaxUpgradesPerStat;
                break;
            case "ü��":
                isMaxed = playerStats.GetHealthCount() >= playerStats.MaxUpgradesPerStat;
                break;
            case "�̵��ӵ�":
                isMaxed = playerStats.GetSpeedCount() >= playerStats.MaxUpgradesPerStat;
                break;
        }
        if (success)
        {
            if (isMaxed)
                UIManager.Instance.ShowDialog(npcName,
                        $"{statName} ��ȭ �Ϸ�!");
            else
                UIManager.Instance.ShowDialog(npcName,
                    $"{statName} ��ȭ ����! ���� �ڽ�Ʈ: {nextCost} ���");
        }
        else
            UIManager.Instance.ShowDialog(npcName,
                $"��ȭ ����! ��尡 �����ϰų� ��� ��ȭ�� �Ϸ��߽��ϴ�!");

        UpdateCostTexts();
    }

    void UpdateCostTexts()
    {
        if (playerStats == null) return;

        goldText.text = $"Gold: {playerStats.GetCurrentGold()}";

        if (playerStats.GetAttackCount() >= playerStats.MaxUpgradesPerStat)
            attackCostText.text = "��ȭ �Ϸ�";
        else
            attackCostText.text = $"���ݷ�: {playerStats.GetAttackCost()} G";

        // ����
        if (playerStats.GetDefenseCount() >= playerStats.MaxUpgradesPerStat)
            defenseCostText.text = "��ȭ �Ϸ�";
        else
            defenseCostText.text = $"����: {playerStats.GetDefenseCost()} G";

        // ü��
        if (playerStats.GetHealthCount() >= playerStats.MaxUpgradesPerStat)
            healthCostText.text = "��ȭ �Ϸ�";
        else
            healthCostText.text = $"ü��: {playerStats.GetHealthCost()} G";

        // �̵��ӵ�
        if (playerStats.GetSpeedCount() >= playerStats.MaxUpgradesPerStat)
            speedCostText.text = "��ȭ �Ϸ�";
        else
            speedCostText.text = $"�̵��ӵ�: {playerStats.GetSpeedCost()} G";
    }

}
