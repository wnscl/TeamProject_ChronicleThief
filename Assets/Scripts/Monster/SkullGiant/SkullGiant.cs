using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullGiant : MonsterAi, IBattleEntity
{
    private void Start()
    {
        FirstSetting();
    }

    public void FirstSetting()
    {
        survive = true;
        isAttacked = false;
        isSpawn = false;
        name = "�������̾�Ʈ";
        moveSpeed = 4.5f;
        hp = 80;
        atk = 25;
        attackRange = 1.9f;
        chaseRange = 12f;
        attackDuration = 1f;

        StartCoroutine(MonsterStateRepeater(MonsterAiState.Spawn));
    }

}
