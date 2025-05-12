using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullRunner : BasicMonster, IBattleEntity
{
    SkullRunnerWeapon weapon;
    GameObject theLine;
    [SerializeField] float runSpeed;
    Vector2 targetPos;

    private void Start()
    {
        weapon = GetComponentInChildren<SkullRunnerWeapon>();
        theLine = transform.Find("Line").gameObject;
        FirstSetting();

        //anim.Play("SkullRunnerIdle", 0, Random.Range(0f, 0.1f));
        //�����Ǵ� ���÷��ʴ� ���� �ִϸ����͸� �����ϱ� ������
        //�����Ǵ� ������ �ٸ����� ��ǥ�� �i�� ��Ȳ�� �� �ִϸ��̼��� �������� ����Ǿ�
        //���Ͱ� ������ �� ���� �Ѹ���ó�� ���̴� ������ �ذ��ϱ� ����
        //�� ���Ͱ� ������ �� ���� start���� �ִϸ��̼��� �����ϴ� ����� ���̵���
        //����Ÿ�̹��� ��߳����ؼ� ������ �� ��������ó�� ���̰� �ϴ� ���
    }

    public void FirstSetting()
    {
        name = "���÷���";
        moveSpeed = 4;
        runSpeed = 7.5f;
        currentHp = 35;
        maxHp = 35;
        atk = 10;

        targetRocate = theStone.transform.position;
        StartAction("startSpawn");
        SetMonsterState(MonsterState.Spawn);
    }

    protected override IEnumerator ChaseCoroutine()
    {
        int readyToNextState = 0;
        StartAction("startMove");

        while (readyToNextState == 0)
        {
            Vector2 nowPos = transform.position;
            Vector2 playerPos = testPlayer.transform.position;
            Vector2 direction = (playerPos - nowPos).normalized;
            Vector2 nextPos = direction * runSpeed * Time.deltaTime;

            float distanceOfPlayer = Vector2.Distance(transform.position, testPlayer.transform.position);

            Debug.DrawLine(transform.position, testPlayer.transform.position, Color.green);


            if (distanceOfPlayer > 10f)
            {
                readyToNextState = 1; //�÷��̾� �߰� ����
                //�ٽ� ������Ʈ �̵� ���� ��ȯ 1
                break;
            }
            else if (distanceOfPlayer <= 2f)
            {
                readyToNextState = 2; //�÷��̾� ���ݹ��� üũ
                //���ݸ��� ��ȯ 2
                targetPos = playerPos;
                Debug.Log("�÷��̾ ����Ÿ������ ����");
                break;
            }
            else
            {
                //rigid.velocity = direction * runSpeed;
                rigid.MovePosition(rigid.position + nextPos);
                rigid.velocity = Vector2.zero;
                //�÷��̾� �߰� ��
            }

            //yield return null;
            yield return new WaitForFixedUpdate(); 
            //MovePosition���� ���� ������
            //�Ƚ��������Ʈ���� �ϴ� ���� �´�.
        }

        switch (readyToNextState)
        {
            case 1:
                StopAction("dontStopMove");
                yield return new WaitForSeconds(0.001f);
                SetMonsterState(MonsterState.MoveRocate);
                break;

            case 2:
                StopAction("stopAll");
                yield return new WaitForSeconds(0.001f);
                SetMonsterState (MonsterState.Attack);
                break;
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        int readyToNextState = 0;
        bool isAttack = false;
        StartAction("startAttack");
        float attackSpeed = 20f;
        float attackTimer = 0f;

        Vector2 startPos = transform.position;
        Vector2 attackDirection = (targetPos - (Vector2)transform.position).normalized;
        //���Ͱ� ��ǥ������ �ٶ󺸴� ���Ⱚ
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        //������ �������� ���ϰ� ������ ��׸�(������)�� ����

        theLine.SetActive(true);
        theLine.transform.rotation = Quaternion.Euler(0, 0, angle);

        while (attackTimer < 0.5f)
        {
            attackTimer += Time.deltaTime;
            yield return null;
        }
        theLine.SetActive(false);
        col.isTrigger = true;

        while (!isAttack)
        {
            Vector2 nextPos = attackDirection * attackSpeed * Time.deltaTime;
            rigid.MovePosition(rigid.position + nextPos);

            if (Vector2.Distance(startPos, transform.position) >= 3.5f )
            {
                isAttack = true;
                readyToNextState = 1; //���� ����
                break;
            }

            yield return new WaitForFixedUpdate();
        }


        switch (readyToNextState)
        {
            case 1:
                StopAction("stopAttack");
                yield return new WaitForSeconds(0.001f);
                SetMonsterState(MonsterState.Chase);
                break;
        }
    }

    public override void StartAction(string action)
    {
        switch (action)
        {
            case "startSpawn":
                anim.SetInteger("StateNum", 10);
                break;
            case "startMove":
                anim.SetInteger("StateNum", 1);
                weapon.MoveSet(true);
                break;
            case "startDamage":
                anim.SetInteger("StateNum", 2);
                break;
            case "startAttack":
                anim.SetInteger("StateNum", 3);
                weapon.AttackSet(true);
                break;
            case "startDead":
                anim.SetInteger("StateNum", 4);
                break;
        }
    }

    public void TakeDamage(IBattleEntity attacker, int dmg)
    {
        currentHp -= dmg;
    }
}


/*bool isDetectedPlayer = false;

while (isDetectedPlayer == false)
{
    float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

    if (distanceFromPlayer <= 3f) // �Ÿ��� 3 ���ϸ� �÷��̾ �����ߴٰ� ��
    {
        isDetectedPlayer = true;
    }
    else
    {
        Debug.Log("�÷��̾ �� ã�ҽ��ϴ�.");
    }

    yield return null;
    //���ؽ�Ʈ = �ƶ�
    //����Ƽ�� �������ӿ��� �ؾ��� �͵��� �پ��ѵ�
    //���Ϲ��� �ڵ尡 ���������� �ٸ� �͵��� ������ �ȵǱ� ������
    //�ڷ�ƾ�� ����Ͽ� �ϸ��� �������� �� ���Ϲ��� ����������
    //���������ӿ� �ٽ� ���Ϲ��� ������� ��
    //�� ���� �����ӿ� ������ �ɾ�� 
    // �׸��� �̰��� yield return null�� ���̺� ����Ʈ��� ����
    //--> �ڷ�ƾ ���� current��� ���� ���¸� ��Ƶδ� ������ �ְ�
    //�׷��� ������ ����� �� ���Ϲ��� ������ �ʾҴٸ�
    //Ŀ��Ʈ���� ��Ƶδ� ���� "yield return null���� ���������"
    //�� ������ ����ɶ��� ���̺�����Ʈ���� ����
}

SetMonsterState(MonsterState.Chase);*/