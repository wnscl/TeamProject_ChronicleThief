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
    public GameObject firstNpc;

    [Header("True or False")]
    [SerializeField] bool isRunning;
    [SerializeField] public bool isFirstGame;
    [SerializeField] bool isInBattle;
    [SerializeField] bool isInReady;
    [SerializeField] bool isInBoss;
    [SerializeField] bool isGameOver;
    [SerializeField] int stageTimer;

    [Header("State")]
    [SerializeField] public int waveCount = 1;
    [SerializeField] Stage currentStage;
    [SerializeField] Stage nextStage;

    public static BattleSystemManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<PlayerController>();
        monsterFactory = GetComponentInChildren<MonsterFactory>();


        nextStage = Stage.InReadyWave;
        isRunning = true;
        isFirstGame = true;
        isInBattle = false;
        isInReady = false;
        isInBoss = false;
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



    public void OnStageStart()
    {
        StartCoroutine(StageRepeater());
    }

    public IEnumerator StageRepeater()
    {
        Debug.Log("스테이지 머신 가동");
        while (isRunning)
        {
            currentStage = nextStage;
            switch (currentStage)
            {
                case Stage.InBattleWave:
                    yield return StartCoroutine(BattleWave());

                    // 10 or 20웨이브 먼저 체크해서 메인NPC만 소환
                    if (waveCount == 10)
                    {
                        UIManager.Instance.SpawnWave10Spawner();
                        yield return StartCoroutine(WaitForMainSpawnerTouch(10));
                    }
                    else if (waveCount == 20)
                    {
                        UIManager.Instance.SpawnFinalSpawner();
                        yield return StartCoroutine(WaitForMainSpawnerTouch(20));
                    }

                    // 강화 npc 소환
                    UIManager.Instance.SpawnWaveSpawner(waveCount);
                    yield return StartCoroutine(WaitForWaveSpawnerTouch());

                    yield return new WaitForSeconds(60f);

                    if (waveCount == 9)
                    {
                        GimmickTrigger gimmickTrigger = FindObjectOfType<GimmickTrigger>();
                        gimmickTrigger.StageOneFloorGimic();
                        // fadeout()
                        // map이동 관련 메서드()
                        // fadein()
                    }

                    waveCount++;
                    nextStage = Stage.InReadyWave;
                    break;

                case Stage.InReadyWave:
                    yield return StartCoroutine(ReadyWave());
                    nextStage = DecideNextStage();
                    break;

                case Stage.GameOver:
                    

                    break;
            }

            yield return null;
        }

        yield break;
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

        if (stageTimer <= 0 && (currentStage == Stage.InReadyWave))
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


    public void CheckEnterDungeon(bool isEnter)
    {
        isFirstGame = isEnter;
    }



    private IEnumerator BattleWave()
    {
        isInBattle = true;
        stageTimer = 0;

        UIManager.Instance.ShowStageTimer();

        while (stageTimer < 10)
        {
            Debug.Log($"{stageTimer}초 경과");
            UIManager.Instance.UpdateStageTimer(60 - stageTimer);
            switch (stageTimer)
            {
                case 0:
                case 10:
                case 20:
                case 30:
                case 40:
                case 50:
                    monsterFactory.OnMakeMonster();
                    yield return new WaitForSeconds(0.001f);
                    Debug.Log("몬스터 공장이 일합니다.");
                    break;
            }

            if (CheckGameOver())
            {
                yield break;
            }

            stageTimer += 1;
            yield return new WaitForSeconds(1);
        }

        stageTimer = 0;
        waveCount++;
        UIManager.Instance.HideStageTimer();
        isInBattle = false;
        yield break;
    }

    private IEnumerator ReadyWave()
    {
        isInReady = true;
        stageTimer = 0;

        while (stageTimer < 3)
        {
            Debug.Log($"준비웨이브 {stageTimer}초경과");
            stageTimer += 1;

            yield return new WaitForSeconds(1);
        }
        stageTimer = 0;
        isInReady = false;
        yield break;
    }

    private IEnumerator BossWave()
    {
        isInReady = true;
        stageTimer = 0;

        while (stageTimer < 60)
        {
            Debug.Log($"보스웨이브 {stageTimer}초경과");
            stageTimer += 1;

            yield return new WaitForSeconds(1);
        }
        stageTimer = 0;
        isInReady = false;
        yield break;
    }

    // 스포너 터치 대기 (일반 웨이브)
    private IEnumerator WaitForWaveSpawnerTouch()
    {
        var sp = UIManager.Instance.currentWaveSpawner;

        // spawner가 생성될 때까지 대기
        while (sp == null)
        {
            yield return null;
            sp = UIManager.Instance.currentWaveSpawner;
        }

        // 플레이어가 밟힌 순간까지 대기
        while (!sp.PlayerTouched)
            yield return null;

        // 이제 밟혔다 → 이 시점부터 1분 카운트다운 시작
    }

    // 스포너 터치 대기 (10/20 웨이브)
    IEnumerator WaitForMainSpawnerTouch(int wave)
    {
        NPCSpawner sp = (wave == 10)
            ? UIManager.Instance.currentWave10Spawner
            : UIManager.Instance.currentFinalSpawner;

        while (sp == null || !sp.PlayerTouched)
            yield return null;
    }

}



/*        if (attacker is PlayerController playerAttacker)
        // attacker가 플레이어인 경우
        // 몬스터가 target
        {
            var stats = playerAttacker.GetComponent<PlayerStats>();
            attackPower = stats.PlayerAttackPower;
        }*/