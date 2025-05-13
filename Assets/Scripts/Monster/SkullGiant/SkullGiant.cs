using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullGiant : MonsterAi
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
        name = "스컬러너";
        moveSpeed = 4.5f;
        hp = 80;
        atk = 18;
        attackRange = 1f;
        chaseRange = 12f;
        attackDuration = 1f;

        StartCoroutine(MonsterStateRepeater(MonsterAiState.Spawn));
    }

}
