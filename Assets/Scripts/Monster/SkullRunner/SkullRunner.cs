using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullRunner : BasicMonster
{

    Rigidbody2D rigid;
    [SerializeField] Animator anim;
    BoxCollider2D col;
    SkullRunnerWeapon weapon;
    GameObject theLine;
    [SerializeField] float runSpeed;
    [SerializeField] int a;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        testPlayer = FindObjectOfType<TestPlayer>().gameObject;
        theStone = FindObjectOfType<TheStone>().gameObject;
        weapon = GetComponentInChildren<SkullRunnerWeapon>();
        theLine = transform.Find("Line").gameObject;
        //transform.Find("�ڽ��̸�")�� ���� �ڽ� �� �̸��� ��ġ�ϴ� ������Ʈ �ϳ��� ã�� ��
        a = anim.GetInteger("StateNum");
    }

    private void Start()
    {
        FirstSetting();
        SetMonsterState(MonsterState.Spawn);
        //anim.Play("SkullRunnerIdle", 0, Random.Range(0f, 0.1f));
        //�����Ǵ� ���÷��ʴ� ���� �ִϸ����͸� �����ϱ� ������
        //�����Ǵ� ������ �ٸ����� ��ǥ�� �i�� ��Ȳ�� �� �ִϸ��̼��� �������� ����Ǿ�
        //���Ͱ� ������ �� ���� �Ѹ���ó�� ���̴� ������ �ذ��ϱ� ����
        //�� ���Ͱ� ������ �� ���� start���� �ִϸ��̼��� �����ϴ� ����� ���̵���
        //����Ÿ�̹��� ��߳����ؼ� ������ �� ��������ó�� ���̰� �ϴ� ���

    }

    private void LateUpdate()
    {
        LookPlayer();
    }

    public void FirstSetting()
    {
        name = "���÷���";
        moveSpeed = 5;
        runSpeed = 7f;
        currentHp = 35;
        maxHp = 35;

        targetRocate = theStone.transform.position;
        anim.SetInteger("StateNum", 10);
        a = anim.GetInteger("StateNum");
    }

    protected override IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("StateNum", 0);
        yield return new WaitForSeconds(0.005f);
        a = anim.GetInteger("StateNum");
        SetMonsterState(MonsterState.MoveRocate);
    }

    protected override IEnumerator MoveRocateCoroutine()
    {
        int readyToNextState = 0;
        anim.SetInteger("StateNum", 1);
        a = anim.GetInteger("StateNum");
        weapon.MoveSet(true);

        while (readyToNextState == 0)
        {
            Vector2 nowPos = transform.position;
            Vector2 direction = (targetRocate - nowPos).normalized;
            Vector2 nextPos = direction * moveSpeed * Time.deltaTime;

            float distanceOfPlayer = Vector2.Distance(transform.position, testPlayer.transform.position);
            float distanceOfStone = Vector2.Distance(transform.position, targetRocate);

            Debug.DrawLine(transform.position, testPlayer.transform.position, Color.red);

            if (distanceOfPlayer <= 7f) // �Ÿ��� 7 ���ϸ� �÷��̾ �����ߴٰ� ��
            {
                Debug.Log("�߰ݸ����ȯ");
                readyToNextState = 1; //�÷��̾� �߰ݸ��� ��ȯ
                break;
            }
            else if (distanceOfStone <= 2f)
            {
                Debug.Log("���ݸ����ȯ");
                readyToNextState = 2; //������Ʈ ���ݸ��� ��ȯ
                break;
            }
            else
            {
                //rigid.velocity = direction * moveSpeed;
                //a = �̿�����Ʈ b = Ÿ�ٷ�����Ʈ b���� a�� ������� �׸��� ��ֶ�����
                rigid.MovePosition(rigid.position + nextPos);
                rigid.velocity = Vector2.zero;
            }

            //yield return null;  
            yield return new WaitForFixedUpdate();
        }

        switch (readyToNextState)
        {
            case 1:
                StopMove();
                yield return new WaitForSeconds(0.005f);
                SetMonsterState(MonsterState.Chase);
                break;

            case 2:
                StopMove();
                yield return new WaitForSeconds(0.005f);
                SetMonsterState(MonsterState.Attack);
                break;
        }
    }

    protected override IEnumerator ChaseCoroutine()
    {
        int readyToNextState = 0;
        anim.SetInteger("StateNum", 1);
        a = anim.GetInteger("StateNum");
        weapon.MoveSet(true);

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
                StopMove();
                yield return new WaitForSeconds(0.005f);
                SetMonsterState(MonsterState.MoveRocate);
                break;

            case 2:
                StopMove();
                yield return new WaitForSeconds(0.005f);
                SetMonsterState (MonsterState.Attack);
                break;
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        int readyToNextState = 0;
        anim.SetInteger("StateNum", 3);
        a = anim.GetInteger("StateNum");

        Vector2 goPos = testPlayer.transform.position;
        Vector2 attackDirection = goPos - (Vector2)transform.position;
        //���Ͱ� �÷��̾ �ٶ󺸴� ���Ⱚ
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        //������ �������� ���ϰ�
        //������ ��׸�(������)�� ����

        while (readyToNextState == 0)
        {
            theLine.SetActive(true);
            theLine.transform.rotation = Quaternion.Euler(0,0,angle);

            //yield return null;
            yield return new WaitForFixedUpdate();
        }

    }

    //protected override IEnumerator DeadCoroutine()
    //{

    //}

    //protected override IEnumerator GetDamageCoroutine()
    //{

    //}

    public void StopMove()
    {
        anim.SetInteger("StateNum", 0);
        a = anim.GetInteger("StateNum");
        rigid.velocity = Vector2.zero;
        weapon.MoveSet(false);
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