using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("��ȭ ����")]
    [SerializeField] private int maxUpgradesPerStat = 5;  // �ִ� ��ȭ Ƚ��
    public float costMultiplier = 1.5f;                  // ��� ���� ���
    public int baseAttackCost = 100;                    // �⺻ ���� ��ȭ ���
    public int baseDefenseCost = 100;                    // �⺻ ��� ��ȭ ���
    public int baseHealthCost = 150;                    // �⺻ ü�� ��ȭ ���
    public int baseSpeedCost = 150;                    // �⺻ �̵��ӵ� ��ȭ ���

    // �ܺο��� ���� �ִ� ��ȭ Ƚ���� ���� �� �ֵ���
    public int MaxUpgradesPerStat => maxUpgradesPerStat;

    private int attackCount, defenseCount, healthCount, speedCount;
    private int attackCost, defenseCost, healthCost, speedCost;
    private int savedBaseAttackCost, savedBaseDefenseCost, savedBaseHealthCost, savedBaseSpeedCost;
    private int savedMaxUpgrades;

    private PlayerStats stats;

    void Awake()
    {
        // PlayerStats ĳ��
        stats = GetComponent<PlayerStats>();

        // ���� ��롤Ƚ���� ���� (���¿�)
        savedBaseAttackCost = baseAttackCost;
        savedBaseDefenseCost = baseDefenseCost;
        savedBaseHealthCost = baseHealthCost;
        savedBaseSpeedCost = baseSpeedCost;
        savedMaxUpgrades = maxUpgradesPerStat;

        // ���� ��� �ʱ�ȭ
        attackCost = baseAttackCost;
        defenseCost = baseDefenseCost;
        healthCost = baseHealthCost;
        speedCost = baseSpeedCost;
    }

    // NPC�� �ı��ǰ� ���ȯ�� �� ȣ���ؼ� ��� ��ȭ ����� �ʱ�ȭ
    public void ResetUpgrades()
    {
        attackCount = defenseCount = healthCount = speedCount = 0;
        attackCost = savedBaseAttackCost;
        defenseCost = savedBaseDefenseCost;
        healthCost = savedBaseHealthCost;
        speedCost = savedBaseSpeedCost;
        maxUpgradesPerStat = savedMaxUpgrades;
    }

    // ���ο��� ����� ������ �Ҹ��ϴ� ����
    private bool TrySpend(int cost)
    {
        return ResourcesHandler.Instance.SpendGold(cost);
    }

    public bool TryUpgradeAttack()
    {
        if (attackCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(attackCost)) return false;

        stats.PlayerAttackPower += 2f;                                 // ���ݷ� ��ȭ
        attackCount++;
        attackCost = Mathf.CeilToInt(attackCost * costMultiplier);     // ���� ��� ����
        return true;
    }

    public bool TryUpgradeDefense()
    {
        if (defenseCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(defenseCost)) return false;

        stats.PlayerDefensePower += 1f;                                // ���� ��ȭ
        defenseCount++;
        defenseCost = Mathf.CeilToInt(defenseCost * costMultiplier);
        return true;
    }

    public bool TryUpgradeHealth()
    {
        if (healthCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(healthCost)) return false;

        // �ִ� ü���� 5�� ���� (HealthSystem�� ����)
        HealthSystem.Instance.SetMaxHealth(50);
        healthCount++;
        healthCost = Mathf.CeilToInt(healthCost * costMultiplier);
        return true;
    }

    public bool TryUpgradeSpeed()
    {
        if (speedCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(speedCost)) return false;

        stats.Speed += 0.2f;                                          // �̵��ӵ� ��ȭ
        speedCount++;
        speedCost = Mathf.CeilToInt(speedCost * costMultiplier);
        return true;
    }

    // �ܺο� getter
    public int GetAttackCost() => attackCost;
    public int GetDefenseCost() => defenseCost;
    public int GetHealthCost() => healthCost;
    public int GetSpeedCost() => speedCost;

    public int GetAttackCount() => attackCount;
    public int GetDefenseCount() => defenseCount;
    public int GetHealthCount() => healthCount;
    public int GetSpeedCount() => speedCount;
}