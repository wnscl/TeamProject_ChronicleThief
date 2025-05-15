using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("강화 설정")]
    [SerializeField] private int maxUpgradesPerStat = 5;  // 최대 강화 횟수
    public float costMultiplier = 1.5f;                  // 비용 증가 계수
    public int baseAttackCost = 100;                    // 기본 공격 강화 비용
    public int baseDefenseCost = 100;                    // 기본 방어 강화 비용
    public int baseHealthCost = 150;                    // 기본 체력 강화 비용
    public int baseSpeedCost = 150;                    // 기본 이동속도 강화 비용

    // 외부에서 현재 최대 강화 횟수를 읽을 수 있도록
    public int MaxUpgradesPerStat => maxUpgradesPerStat;

    private int attackCount, defenseCount, healthCount, speedCount;
    private int attackCost, defenseCost, healthCost, speedCost;
    private int savedBaseAttackCost, savedBaseDefenseCost, savedBaseHealthCost, savedBaseSpeedCost;
    private int savedMaxUpgrades;

    private PlayerStats stats;

    void Awake()
    {
        // PlayerStats 캐싱
        stats = GetComponent<PlayerStats>();

        // 최초 비용·횟수값 저장 (리셋용)
        savedBaseAttackCost = baseAttackCost;
        savedBaseDefenseCost = baseDefenseCost;
        savedBaseHealthCost = baseHealthCost;
        savedBaseSpeedCost = baseSpeedCost;
        savedMaxUpgrades = maxUpgradesPerStat;

        // 현재 비용 초기화
        attackCost = baseAttackCost;
        defenseCost = baseDefenseCost;
        healthCost = baseHealthCost;
        speedCost = baseSpeedCost;
    }

    // NPC가 파괴되고 재소환될 때 호출해서 모든 강화 기록을 초기화
    public void ResetUpgrades()
    {
        attackCount = defenseCount = healthCount = speedCount = 0;
        attackCost = savedBaseAttackCost;
        defenseCost = savedBaseDefenseCost;
        healthCost = savedBaseHealthCost;
        speedCost = savedBaseSpeedCost;
        maxUpgradesPerStat = savedMaxUpgrades;
    }

    // 내부에서 비용을 실제로 소모하는 헬퍼
    private bool TrySpend(int cost)
    {
        return ResourcesHandler.Instance.SpendGold(cost);
    }

    public bool TryUpgradeAttack()
    {
        if (attackCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(attackCost)) return false;

        stats.PlayerAttackPower += 2f;                                 // 공격력 강화
        attackCount++;
        attackCost = Mathf.CeilToInt(attackCost * costMultiplier);     // 다음 비용 갱신
        return true;
    }

    public bool TryUpgradeDefense()
    {
        if (defenseCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(defenseCost)) return false;

        stats.PlayerDefensePower += 1f;                                // 방어력 강화
        defenseCount++;
        defenseCost = Mathf.CeilToInt(defenseCost * costMultiplier);
        return true;
    }

    public bool TryUpgradeHealth()
    {
        if (healthCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(healthCost)) return false;

        // 최대 체력을 5씩 증가 (HealthSystem과 연동)
        HealthSystem.Instance.SetMaxHealth(50);
        healthCount++;
        healthCost = Mathf.CeilToInt(healthCost * costMultiplier);
        return true;
    }

    public bool TryUpgradeSpeed()
    {
        if (speedCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(speedCost)) return false;

        stats.Speed += 0.2f;                                          // 이동속도 강화
        speedCount++;
        speedCost = Mathf.CeilToInt(speedCost * costMultiplier);
        return true;
    }

    // 외부용 getter
    public int GetAttackCost() => attackCost;
    public int GetDefenseCost() => defenseCost;
    public int GetHealthCost() => healthCost;
    public int GetSpeedCost() => speedCost;

    public int GetAttackCount() => attackCount;
    public int GetDefenseCount() => defenseCount;
    public int GetHealthCount() => healthCount;
    public int GetSpeedCount() => speedCount;
}