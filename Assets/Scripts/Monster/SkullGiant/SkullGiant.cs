using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullGiant : BasicMonster, IBattleEntity
{
    private void Start()
    {
        FirstSetting();
    }

    public void FirstSetting()
    {
        name = "스컬자이언트";
        moveSpeed = 4.5f;
        runSpeed = 3.5f;
        currentHp = 80;
        maxHp = 80;
        atk = 18;
        detectDistancePlayer = 5f;
        detectDistanceStone = 2.2f;
        failDistancePlayer = 7f;
        attackDistance = 1.6f;

        targetRocate = theStone.transform.position;
        StartAction("startSpawn");
        SetMonsterState(MonsterState.Spawn);
    }

    public void TakeDamage(IBattleEntity attacker, int dmg)
    {
        currentHp -= dmg;
    }
}
