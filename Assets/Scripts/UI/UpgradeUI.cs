using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    [Header("�г�")]
    public GameObject panel;       // ��ȭ UI ��ü �г�
    public TMP_Text goldText;    // ���� ��� ǥ��

    [Header("��ȭ ��ư")]
    public Button attackBtn;
    public Button defenseBtn;
    public Button healthBtn;
    public Button speedBtn;

    [Header("��� �ؽ�Ʈ")]
    public TMP_Text attackCostText;
    public TMP_Text defenseCostText;
    public TMP_Text healthCostText;
    public TMP_Text speedCostText;

    [Header("���� ������Ʈ")]
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

        // ��ư Ŭ�� �� �׻� TryUpgrade�� ȣ�� (Show���� ���� ����)
        attackBtn.onClick.AddListener(() => TryUpgrade(StatType.Attack));
        defenseBtn.onClick.AddListener(() => TryUpgrade(StatType.Defense));
        healthBtn.onClick.AddListener(() => TryUpgrade(StatType.Health));
        speedBtn.onClick.AddListener(() => TryUpgrade(StatType.Speed));

        UpdateAllTexts();
    }

    // ��ȭ UI ����
    public void Show()
    {
        panel.SetActive(true);

        // �ݾҴ� ���, ���� count�� �״�� ����ؼ� ���ͷ�Ƽ�� ����
        attackBtn.interactable = upgradeManager.GetAttackCount() < upgradeManager.MaxUpgradesPerStat;
        defenseBtn.interactable = upgradeManager.GetDefenseCount() < upgradeManager.MaxUpgradesPerStat;
        healthBtn.interactable = upgradeManager.GetHealthCount() < upgradeManager.MaxUpgradesPerStat;
        speedBtn.interactable = upgradeManager.GetSpeedCount() < upgradeManager.MaxUpgradesPerStat;

        UpdateAllTexts();
    }

    // ��ȭ �õ�
    private void TryUpgrade(StatType stat)
    {
        int cost = 0;
        bool isMax = false;

        // ��롤�ִ�ġ ���� ��ȸ
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
            UIManager.Instance.ShowDialog("��ȭ��", "�̹� �ִ� ��ȭġ�Դϴ�!");
            return;
        }

        if (!resourcesHandler.SpendGold(cost))
        {
            UIManager.Instance.ShowDialog("��ȭ��", "��尡 �����մϴ�!");
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

        UIManager.Instance.ShowDialog("��ȭ��", success ? "��ȭ ����!" : "��ȭ ����");

        // ��ȭ ���� ���ο� �������, ��ư ���´� Show() �� �ٽ� ����
        UpdateAllTexts();
    }

    // UI �ؽ�Ʈ ������Ʈ
    private void UpdateAllTexts()
    {
        attackCostText.text = $"���ݷ�: {(upgradeManager.GetAttackCount() >= upgradeManager.MaxUpgradesPerStat ? "�Ϸ�" : upgradeManager.GetAttackCost() + "G")}";
        defenseCostText.text = $"����: {(upgradeManager.GetDefenseCount() >= upgradeManager.MaxUpgradesPerStat ? "�Ϸ�" : upgradeManager.GetDefenseCost() + "G")}";
        healthCostText.text = $"ü��: {(upgradeManager.GetHealthCount() >= upgradeManager.MaxUpgradesPerStat ? "�Ϸ�" : upgradeManager.GetHealthCost() + "G")}";
        speedCostText.text = $"�̵��ӵ�: {(upgradeManager.GetSpeedCount() >= upgradeManager.MaxUpgradesPerStat ? "�Ϸ�" : upgradeManager.GetSpeedCost() + "G")}";

        goldText.text = $"Gold: {resourcesHandler.Gold}G";
    }

    // ��ȭ UI �ݱ�
    public void Hide()
    {
        panel.SetActive(false);
    }
}
