using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public interface IBattleEntity
{
    void TakeDamage(IBattleEntity attacker, int dmg);
}

public class BattleSystemManager : MonoBehaviour
{
    [SerializeField] PlayerController player;

    [SerializeField] MonsterAi mob;
    [SerializeField] int testAtk;

    public static BattleSystemManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<PlayerController>();
    }


    public void AttackOther(IBattleEntity attacker, IBattleEntity target)
    {
        float attackPower = 10f; // fallback
        // attacker가 플레이어인 경우
        if (attacker != null && attacker is MonsterAi)
        {
            mob = attacker as MonsterAi;
        }

        if (attacker is PlayerController playerAttacker)
        {
            var stats = playerAttacker.GetComponent<PlayerStats>();
            attackPower = stats.PlayerAttackPower;
        }

        // attacker가 몬스터인 경우
        else if (attacker is MonsterAi monsterAttacker)
        {
            testAtk = monsterAttacker.Atk;
            attackPower = monsterAttacker.Atk; // protected라서 public 프로퍼티 추가 필요함 public int Atk => atk;
        }

        float finalDamage = attackPower;

        // 타겟이 플레이어시 방어력 적용
        if (target is PlayerController playerTarget)
        {
            var stats = playerTarget.GetComponent<PlayerStats>();
            float defense = stats.PlayerDefensePower;

            finalDamage = attackPower * (100f / (100f + defense));
            finalDamage = Mathf.Max(1, finalDamage);
        }

        target.TakeDamage(attacker, Mathf.RoundToInt(finalDamage));
    }

    public void AttackPlayer(int dmg)
    {
        player.TakeDamage(dmg);
    }
}
