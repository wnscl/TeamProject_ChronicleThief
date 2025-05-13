using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


enum Stage
{
    None,
    FirstStart,
    InBattleWave,
    InReadyWave,
    GameOver
}

public interface IBattleEntity
{
    void TakeDamage(IBattleEntity attacker, int dmg);
}

public class BattleSystemManager : MonoBehaviour
{
    [Header("Basic Field")]
    [SerializeField] PlayerController player;
    [SerializeField] MonsterFactory monsterFactory;

    [Header("True or False")]
    [SerializeField] bool isRunning;
    [SerializeField] bool isFirstGame;
    [SerializeField] bool isInBattle;
    [SerializeField] bool isInReady;
    [SerializeField] bool isGameOver;
    [SerializeField] int stageTimer;

    [Header("State")]
    [SerializeField] Stage currentStage;
    [SerializeField] Stage nextStage;

    public static BattleSystemManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<PlayerController>();
        monsterFactory = GetComponentInChildren<MonsterFactory>();


        nextStage = Stage.FirstStart;
        isRunning = true;
        isFirstGame = true;
        isInBattle = false;
        isInReady = false;
        isGameOver = false;

    }

    public void AttackOther(IBattleEntity attacker, IBattleEntity target)
    {
        float attackPower= 0;

        var stats = player.GetComponent<PlayerStats>();
        attackPower = stats.PlayerAttackPower;

        float finalDamage = attackPower;

        Debug.Log($"플레이어 -> 몬스터 최종데미지 : {finalDamage}");
        target.TakeDamage(attacker, Mathf.RoundToInt(finalDamage));
    }
    public void AttackPlayer(int dmg)
    {
        float finalDamage;

        var stats = player.GetComponent<PlayerStats>();
        float defense = stats.PlayerDefensePower;

        finalDamage = dmg * (100f / (100f + defense));
        finalDamage = Mathf.Max(1, finalDamage);
        Debug.Log($"몬스터 -> 플레이어 최종데미지 : {finalDamage}");
        player.TakeDamage(Mathf.RoundToInt(finalDamage));
    }





    protected IEnumerator StageRepeater()
    {
        Debug.Log("스테이지 머신 가동");
        while (isRunning)
        {
            currentStage = nextStage;
            switch (currentStage)
            {
                case Stage.FirstStart:


                    break;

                case Stage.InBattleWave:
                    yield return StartCoroutine(BattleWave());
                    nextStage = DecideNextStage();
                    break;

                case Stage.InReadyWave:
                    yield return StartCoroutine(ReadyWave());
                    nextStage = DecideNextStage();
                    break;

                case Stage.GameOver:
                    

                    break;
            }
        }
    }
    private Stage DecideNextStage()
    {
        if (CheckGameOver())
        {
            return Stage.GameOver;
        }

        if (stageTimer <= 0 && currentStage == Stage.InBattleWave)
        {
            return Stage.InReadyWave;
        }

        if (stageTimer <= 0 && currentStage == Stage.InReadyWave)
        {
            return Stage.InBattleWave;
        }

        return Stage.None;
    }
    private bool CheckGameOver()
    {
        if (player.IsDead && isInBattle)
        {
            isGameOver = true;
        }

        return isGameOver;
    }

    private IEnumerator BattleWave()
    {
        isInBattle = true;
        stageTimer = 60;

        while (stageTimer > 0)
        {
            stageTimer--;
            //UIManager.Instance.시간흐르는메서드호출(stageTimer)
            if (stageTimer % 10 == 0)
            {
                monsterFactory.OnMakeMonster();
                
            }

            if (CheckGameOver())
            {
                yield break;
            }

            yield return new WaitForSeconds(1);
        }

        isInBattle = false;
        yield break;
    }

    private IEnumerator ReadyWave()
    {
        isInReady = true;
        stageTimer = 60;

        while (stageTimer > 0)
        {
            stageTimer--;

            if (stageTimer % 10 == 0)
            {
                monsterFactory.OnMakeMonster();
            }
            yield return new WaitForSeconds(1);
        }

        isInReady = false;
        yield break;
    }

    private IEnumerator BossWave()
    {

        yield break ;
    }

}



/*        if (attacker is PlayerController playerAttacker)
        // attacker가 플레이어인 경우
        // 몬스터가 target
        {
            var stats = playerAttacker.GetComponent<PlayerStats>();
            attackPower = stats.PlayerAttackPower;
        }*/