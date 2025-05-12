using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(PlayerStats), typeof(ResourcesHandler))]
public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("강화 설정")]
    [SerializeField] private int maxUpgradesPerStat = 5;
    [SerializeField] private float costMultiplier = 1.5f;

    [Header("기본 비용 (1회차)")]
    [SerializeField] private int baseAttackCost = 100;
    [SerializeField] private int baseDefenseCost = 100;
    [SerializeField] private int baseHealthCost = 150;
    [SerializeField] private int baseSpeedCost = 150;

    // 내부 추적
    private int attackCount, defenseCount, healthCount, speedCount;
    private int attackCost, defenseCost, healthCost, speedCost;

    private PlayerStats stats;
    private ResourcesHandler resources;
    private FieldInfo goldField;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        resources = GetComponent<ResourcesHandler>();

        // private int gold 필드 리플렉션으로 찾기
        goldField = typeof(ResourcesHandler)
                    .GetField("gold",
                              BindingFlags.Instance | BindingFlags.NonPublic);

        // 초기 비용 세팅
        attackCost = baseAttackCost;
        defenseCost = baseDefenseCost;
        healthCost = baseHealthCost;
        speedCost = baseSpeedCost;
    }

    bool TrySpend(int cost)
    {
        int currentGold = resources.Gold;
        if (currentGold < cost) return false;
        // private field 'gold' -= cost
        goldField.SetValue(resources, currentGold - cost);
        return true;
    }

    public bool TryUpgradeAttack()
    {
        if (attackCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(attackCost)) return false;

        stats.PlayerAttackPower += 2f;
        attackCount++;
        attackCost = Mathf.CeilToInt(attackCost * costMultiplier);
        return true;
    }

    public bool TryUpgradeDefense()
    {
        if (defenseCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(defenseCost)) return false;

        stats.PlayerDefensePower += 1f;
        defenseCount++;
        defenseCost = Mathf.CeilToInt(defenseCost * costMultiplier);
        return true;
    }

    public bool TryUpgradeHealth()
    {
        if (healthCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(healthCost)) return false;

        stats.Health += 20;
        healthCount++;
        healthCost = Mathf.CeilToInt(healthCost * costMultiplier);
        return true;
    }

    public bool TryUpgradeSpeed()
    {
        if (speedCount >= maxUpgradesPerStat) return false;
        if (!TrySpend(speedCost)) return false;

        stats.Speed += 0.5f;
        speedCount++;
        speedCost = Mathf.CeilToInt(speedCost * costMultiplier);
        return true;
    }

    // UpgradeUI에서 호출할 메서드들
    public int GetAttackCost() => attackCost;
    public int GetDefenseCost() => defenseCost;
    public int GetHealthCost() => healthCost;
    public int GetSpeedCost() => speedCost;
    public int GetCurrentGold() => resources.Gold;
    public int GetAttackCount() => attackCount;
    public int GetDefenseCount() => defenseCount;
    public int GetHealthCount() => healthCount;
    public int GetSpeedCount() => speedCount;

    public int MaxUpgradesPerStat => maxUpgradesPerStat;
}
