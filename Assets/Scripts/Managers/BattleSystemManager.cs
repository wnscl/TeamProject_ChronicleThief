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
        target.TakeDamage(attacker, 10);
    }
}
