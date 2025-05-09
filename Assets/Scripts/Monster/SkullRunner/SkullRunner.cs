using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullRunner : BasicMonster
{

    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D col;
    GameObject testPlayer;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        testPlayer = FindObjectOfType<TestPlayer>().gameObject;

        FirstSetting();

        SetMonsterState(MonsterState.Spawn);
        anim.SetBool("isSpawn", true);

    }

    public void FirstSetting()
    {
        moveSpeed = 5;
        targetRocate = new Vector2(-30, -50);
    }



    protected override IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("isSpawn", false);
        SetMonsterState(MonsterState.MoveRocate);
    }

    protected override IEnumerator MoveRocateCoroutine()
    {
        bool isDetectedPlayer = false;
        anim.SetBool("isMove", true);

        while (!isDetectedPlayer)
        {
            Vector2 nowPos = transform.position;
            direction = (targetRocate - nowPos).normalized;

            float distanceOfPlayer = Vector2.Distance(transform.position, testPlayer.transform.position);

            Debug.DrawLine(transform.position, testPlayer.transform.position, Color.red);

            if (distanceOfPlayer <= 5f) // �Ÿ��� 5 ���ϸ� �÷��̾ �����ߴٰ� ��
            {
                isDetectedPlayer = true;
            }
            else
            {
                rigid.velocity = direction * moveSpeed;
                //a = �̿�����Ʈ b = Ÿ�ٷ�����Ʈ b���� a�� ������� �׸��� ��ֶ�����
            }

            yield return null;
        }
        
        SetMonsterState(MonsterState.Chase);
        rigid.velocity = Vector2.zero;
        anim.SetBool("isMove", false);
    }

    protected override IEnumerator ChaseCoroutine()
    {
        bool isAttackPlayer = false;
        anim.SetBool("isMove", true);

        /*while (!isAttackPlayer)
        {
            //float distanceOfPlayer = Vector2.Distance()
        }*/
        yield return null;
    }

    //protected override IEnumerator AttackCoroutine()
    //{

    //}

    //protected override IEnumerator DeadCoroutine()
    //{

    //}

    //protected override IEnumerator GetDamageCoroutine()
    //{

    //}
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