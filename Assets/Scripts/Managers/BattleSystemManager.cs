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

                    // 10 or 20���̺� ���� üũ�ؼ� ����NPC�� ��ȯ
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

                    // ��ȭ npc ��ȯ
                    UIManager.Instance.SpawnWaveSpawner(waveCount);
                    yield return StartCoroutine(WaitForWaveSpawnerTouch());

                    yield return new WaitForSeconds(60f);

                    if (waveCount == 9)
                    {
                        GimmickTrigger gimmickTrigger = FindObjectOfType<GimmickTrigger>();
                        gimmickTrigger.StageOneFloorGimic();
                        // fadeout()
                        // map�̵� ���� �޼���()
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
                    monsterFactory.OnMakeMonster();
                    yield return new WaitForSeconds(0.001f);
                    Debug.Log("���� ������ ���մϴ�.");
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
            Debug.Log($"�غ���̺� {stageTimer}�ʰ��");
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
            Debug.Log($"�������̺� {stageTimer}�ʰ��");
            stageTimer += 1;

            yield return new WaitForSeconds(1);
        }
        stageTimer = 0;
        isInReady = false;
        yield break;
    }

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
    }

    // ������ ��ġ ��� (10/20 ���̺�)
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
        // attacker�� �÷��̾��� ���
        // ���Ͱ� target
        {
            var stats = playerAttacker.GetComponent<PlayerStats>();
            attackPower = stats.PlayerAttackPower;
        }*/