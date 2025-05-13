using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullRunner : MonsterAi, IBattleEntity
{
    GameObject theLine;



    private void Start()
    {
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
        survive = true;
        isAttacked = false;
        isSpawn = false;
        name = "���÷���";
        moveSpeed = 4;
        hp = 40;
        atk = 10;
        attackRange = 1f;
        chaseRange = 12f;

        StartCoroutine(MonsterStateRepeater(MonsterAiState.Spawn));
    }


    /*    protected override IEnumerator AttackCoroutine()
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
        }*/


    public void TakeDamage(IBattleEntity attacker, int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            survive = false;
            isAttacked = true;
        }
        else
        {
            isAttacked = true;
        }
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