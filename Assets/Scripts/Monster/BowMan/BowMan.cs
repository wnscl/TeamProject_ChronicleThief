using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : RangeMonsterAi, IBattleEntity
{

    private void Start()
    {
        stone = FindObjectOfType<TheStone>();

        FirstSetting();
        
    }

    public void FirstSetting()
    {

        survive = true;
        isAttacked = false;
        isSpawn = false;
        name = "º¸¿ì¸Ç";
        moveSpeed = 4;
        hp = 150;
        atk = 30;
        attackDuration = 1f;
        arrowSpeed = 20f;
        spawnPoint = transform.position;
        mobGold = 150 + (BattleSystemManager.Instance.waveCount * 5);

        StartCoroutine(MonsterStateRepeater(MonsterAiState.Spawn));
    }

}
