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
    In5Wave,
    In10Wave,
    In20Wave,
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
    [SerializeField] bool boss1;
    [SerializeField] bool boss2;
    [SerializeField] bool boss3;
    [SerializeField] int bossCount;

    [Header("State")]
    [SerializeField] public int waveCount = 0;
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

        Debug.Log($"�÷��̾� -> ���� ���������� : {finalDamage}");
        target.TakeDamage(attacker, Mathf.RoundToInt(finalDamage));
    }
    public void AttackPlayer(int dmg)
    {
        float finalDamage;

        var stats = player.GetComponent<PlayerStats>();
        float defense = stats.PlayerDefensePower;

        finalDamage = dmg * (100f / (100f + defense));
        finalDamage = Mathf.Max(1, finalDamage);
        Debug.Log($"���� -> �÷��̾� ���������� : {finalDamage}");
        player.TakeDamage(Mathf.RoundToInt(finalDamage));
    }



    public void OnStageStart()
    {
        StartCoroutine(StageRepeater());
    }

    public IEnumerator StageRepeater()
    {
        Debug.Log("�������� �ӽ� ����");
        while (isRunning)
        {
            currentStage = nextStage;
            switch (currentStage)
            {
                case Stage.InBattleWave:
                    yield return StartCoroutine(BattleWave());

                    Debug.Log($"{waveCount}wave ��.");

                    // ��ȭ npc ��ȯ
                    if (waveCount != 20)
                    {
                        UIManager.Instance.SpawnWaveSpawner(waveCount);
                        yield return StartCoroutine(WaitForWaveSpawnerTouch());
                    }

                    yield return new WaitForSeconds(5f);

                    if (waveCount == 9)
                    {
                        GimmickTrigger gimmickTrigger = FindObjectOfType<GimmickTrigger>();
                        gimmickTrigger.StageOneFloorGimic();
                        yield return new WaitForSeconds(3f);
                        // fadeout()

                        StageManager.instance.FloorChange(StageManager.instance.player);
                        // fadein()
                    }

                    //if (waveCount == 19)
                    //{
                    //    StageManager.instance.FloorChange(StageManager.instance.player);
                    //}
                    nextStage = DecideNextStage();
                    break;

                case Stage.InReadyWave:
                    waveCount++;
                    Debug.Log($"{waveCount}wave ����.");
                    yield return StartCoroutine(ReadyWave());
                    nextStage = DecideNextStage();
                    break;

                case Stage.In5Wave:
                    yield return StartCoroutine(BossBattleWave());
                    UIManager.Instance.SpawnWaveSpawner(waveCount);
                    yield return StartCoroutine(WaitForWaveSpawnerTouch());
                    yield return new WaitForSeconds(1f);
                    Debug.Log($"{waveCount}wave ��.");
                    nextStage = DecideNextStage();
                    break;

                case Stage.In10Wave:
                    yield return StartCoroutine(BossBattleWave());
                    StageManager.instance.FloorChange(StageManager.instance.player);
                    // fadeout();
                    // yield return new WaitForSeconds(3f);
                    // fadein();

                    UIManager.Instance.SpawnWaveSpawner(waveCount);
                    yield return StartCoroutine(WaitForWaveSpawnerTouch());
                    UIManager.Instance.SpawnWave10Spawner();
                    yield return StartCoroutine(WaitForWave10SpawnerTouch());


                    yield return new WaitForSeconds(1f);

                    nextStage = DecideNextStage();
                    break;

                case Stage.In20Wave:
                    yield return new WaitForSeconds(30f);
                    yield return StartCoroutine(BossBattleWave());
                    //UIManager.Instance.SpawnFinalSpawner();
                    //yield return StartCoroutine(WaitForMainSpawnerTouch(20));
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

        if (boss1)
        {
            bossCount = 1;
            boss1 = false;
            return Stage.In5Wave;
        }
        if (boss2)
        {
            bossCount = 2;
            boss2 = false;
            return Stage.In10Wave;
        }
        if (boss3)
        {
            bossCount = 3;
            boss3 = false;
            return Stage.In20Wave;
        }

        if (stageTimer <= 0 && (currentStage == Stage.InBattleWave ||
            currentStage == Stage.In5Wave ||
            currentStage == Stage.In10Wave ||
            currentStage == Stage.In20Wave))
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


    private IEnumerator BossBattleWave()
    {
        if (waveCount == 20)
        {
            yield break;
        }
        isInBattle = true;
        stageTimer = 0;

        UIManager.Instance.ShowStageTimer();
        monsterFactory.OnMakeBossMonster(bossCount);

        while (stageTimer < 1)
        {
            Debug.Log($"{stageTimer}�� ���");
            UIManager.Instance.UpdateStageTimer(60 - stageTimer);

            if (CheckGameOver())
            {
                yield break;
            }

            stageTimer += 1;
            yield return new WaitForSeconds(1);
        }

        stageTimer = 0;
        UIManager.Instance.HideStageTimer();
        isInBattle = false;
        yield break;
    }


    private IEnumerator BattleWave()
    {
        isInBattle = true;
        stageTimer = 0;

        UIManager.Instance.ShowStageTimer();

        while (stageTimer < 1)
        {
            Debug.Log($"{stageTimer}�� ���");
            UIManager.Instance.UpdateStageTimer(60 - stageTimer);
            switch (stageTimer)
            {
                case 0:
                case 10:
                case 20:
                case 30:
                case 40:
                case 50:
                    if (waveCount > 10)
                    {
                        monsterFactory.OnMakeStage2Monster();
                        yield return new WaitForSeconds(0.001f);
                        Debug.Log("���� ������ ���մϴ�.");
                    }
                    else
                    {
                        monsterFactory.OnMakeMonster();
                        yield return new WaitForSeconds(0.001f);
                        Debug.Log("���� ������ ���մϴ�.");
                    }
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
        UIManager.Instance.HideStageTimer();
        isInBattle = false;
        yield break;
    }

    private IEnumerator ReadyWave()
    {
        isInReady = true;
        stageTimer = 0;

        if (waveCount == 5)
        {
            boss1 = true;
        }
        if (waveCount == 10)
        {
            boss2 = true;
        }
        if (waveCount == 20)
        {
            boss3 = true;
        }

        while (stageTimer < 1)
        {
            Debug.Log($"�غ���̺� {stageTimer}�ʰ��");
            stageTimer += 1;

            yield return new WaitForSeconds(1);
        }
        stageTimer = 0;
        isInReady = false;
        yield break;
    }

    //private IEnumerator BossWave()
    //{
    //    isInReady = true;
    //    stageTimer = 0;

    //    while (stageTimer < 60)
    //    {
    //        Debug.Log($"�������̺� {stageTimer}�ʰ��");
    //        stageTimer += 1;

    //        yield return new WaitForSeconds(1);
    //    }
    //    stageTimer = 0;
    //    isInReady = false;
    //    yield break;
    //}

    // ������ ��ġ ��� (�Ϲ� ���̺�)
    private IEnumerator WaitForWaveSpawnerTouch()
    {
        var sp = UIManager.Instance.currentWaveSpawner;

        // spawner�� ������ ������ ���
        while (sp == null)
        {
            yield return null;
            sp = UIManager.Instance.currentWaveSpawner;
        }

        // �÷��̾ ���� �������� ���
        while (!sp.PlayerTouched)
            yield return null;

        // ���� ������ �� �� �������� 1�� ī��Ʈ�ٿ� ����
        Destroy(sp.gameObject);
    }

    // ������ ��ġ ��� (10���̺�)
    private IEnumerator WaitForWave10SpawnerTouch()
    {
        var sp = UIManager.Instance.currentWave10Spawner;
        while (sp == null)
        {
            yield return null;
            sp = UIManager.Instance.currentWave10Spawner;
        }
        while (!sp.PlayerTouched)
            yield return null;
        Destroy(sp.gameObject);
    }

}



/*        if (attacker is PlayerController playerAttacker)
        // attacker�� �÷��̾��� ���
        // ���Ͱ� target
        {
            var stats = playerAttacker.GetComponent<PlayerStats>();
            attackPower = stats.PlayerAttackPower;
        }*/