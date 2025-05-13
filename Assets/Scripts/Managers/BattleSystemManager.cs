using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleEntity
{
    void TakeDamage(IBattleEntity attacker, int dmg);
}

public class BattleSystemManager : MonoBehaviour
{
    public static BattleSystemManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AttackOther(IBattleEntity attacker, IBattleEntity target)
    {
        float attackPower = 10f; // fallback

        // attacker�� �÷��̾��� ���
        if (attacker is PlayerController playerAttacker)
        {
            var stats = playerAttacker.GetComponent<PlayerStats>();
            attackPower = stats.PlayerAttackPower;
        }

        // attacker�� ������ ���
        else if (attacker is BasicMonster monsterAttacker)
        {
            attackPower = monsterAttacker.Atk; // protected�� public ������Ƽ �߰� �ʿ��� public int Atk => atk;
        }

        float finalDamage = attackPower;

        // Ÿ���� �÷��̾�� ���� ����
        if (target is PlayerController playerTarget)
        {
            var stats = playerTarget.GetComponent<PlayerStats>();
            float defense = stats.PlayerDefensePower;

            finalDamage = attackPower * (100f / (100f + defense));
            finalDamage = Mathf.Max(1, finalDamage);
        }

        target.TakeDamage(attacker, Mathf.RoundToInt(finalDamage));
    }
}
