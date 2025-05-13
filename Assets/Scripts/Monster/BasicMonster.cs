using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum MonsterState
{
    None,
    Spawn, // ���� ���� �� ���� (�ʱ�ȭ)
    GetDamage, // �÷��̾ ������ ���� ������ִ� ���� ���Ϳ� ���� �ٸ�
    MoveRocate, //������ ��ǥ�������� �̵� �׷��� �̵� �� �÷��̾� �߰� �� ü�̽��� ���� ��ȯ
    Chase, //�÷��̾ �߰��� �i�ƿ��� ����
    Attack, //���� �� �޽����� ��ȯ ���Ϳ� ���� �ٸ�
    Dead // ����� ��� �ൿ ���� 
}

public abstract class BasicMonster : MonoBehaviour
{
    private MonsterState CurrentState = MonsterState.Spawn;
    //private MonsterState NextState = MonsterState.None;

    [Header("stat")]
    [SerializeField] protected string name;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int atk;
    public int Atk => atk;
    [SerializeField] protected bool isAlive;

    [Header("attack stat")]
    protected float atkSpeed;
    [SerializeField] protected float attackDistance;
    protected bool isTakingDmg = false;

    [Header("move")]
    [SerializeField] protected Vector2 targetRocate;
    protected Vector2 targetPos;
    [SerializeField] protected float detectDistanceStone;
    [SerializeField] protected float detectDistancePlayer;
    [SerializeField] protected float failDistancePlayer;


    [Header("basic field")]
    protected Rigidbody2D rigid;
    [SerializeField] protected Animator anim;
    protected BoxCollider2D col;
    protected GameObject player;
    protected GameObject theStone;
    protected MonsterMeleeWeapon weapon;
    protected Coroutine currentCoroutine;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
        theStone = FindObjectOfType<TheStone>().gameObject;
        weapon = GetComponentInChildren<MonsterMeleeWeapon>();
    }

    private void LateUpdate()
    {
        LookObject();
    }

    protected virtual void LookObject()
    {
        if (player != null)
        {
            if (transform.position.x > player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                //transform.localScale = new Vector3(-1, 1, 1); // ���� ����
                //���ý����Ϸε� ��������
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                //transform.localScale = new Vector3(1, 1, 1); // ������ ����
                //���ý����Ϸε� ��������
            }
        }
    }

    public void SetMonsterState(MonsterState newstate)
    {
/*        if (CurrentState == state && state == MonsterState.GetDamage)
        {
            
        }*/

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(StateCoroutine(newstate));

/*        switch (CurrentState)
        {
            case MonsterState.Spawn:
                OnSpawn();
                break;

            case MonsterState.GetDamage:
                OnGetDamage();
                break;

            case MonsterState.MoveRocate:
                OnMoveRocate();
                break;

            case MonsterState.Chase:
                OnChase();
                break;

            case MonsterState.Attack:
                OnAttack();
                break;

            case MonsterState.Dead:
                OnDead();
                break;
        }*/
    }

    protected IEnumerator StateCoroutine(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Spawn:
                yield return StartCoroutine(SpawnCoroutine());
                break;

            case MonsterState.GetDamage:
                yield return StartCoroutine(GetDamageCoroutine());
                break;

            case MonsterState.MoveRocate:
                yield return StartCoroutine(MoveRocateCoroutine());
                break;

            case MonsterState.Chase:
                yield return StartCoroutine(ChaseCoroutine());
                break;

            case MonsterState.Attack:
                yield return StartCoroutine(AttackCoroutine());
                break;

            case MonsterState.Dead:
                yield return StartCoroutine(DeadCoroutine());
                break;
        }
    }

    private void OnSpawn()
    {
        Debug.Log("���� ������ȯ");
        StartCoroutine(SpawnCoroutine());
    }
    protected virtual IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        StopAction("stopSpawn");
        yield return new WaitForSeconds(0.001f);
        SetMonsterState(MonsterState.MoveRocate);
    }

    private void OnGetDamage()
    {
        //idle �ִϸ��̼� ���
        //idle�� ���� �ؾ� �� �͵��� ó��
        //Debug.Log("���÷��� ������ ������ȯ");
        StartCoroutine(GetDamageCoroutine());
    }
    protected virtual IEnumerator GetDamageCoroutine()
    {
        isTakingDmg = true;

        StopAction("stopAll");
        yield return new WaitForSeconds(0.001f);

        StartAction("startDamage");
        yield return new WaitForSeconds(0.5f);

        isTakingDmg = false;

        StopAction("stopAll");
        yield return new WaitForSeconds(0.001f);
        SetMonsterState(MonsterState.MoveRocate);
        yield break;
    }

    private void OnMoveRocate()
    {
        Debug.Log("���������Ʈ ������ȯ");
        StartCoroutine(MoveRocateCoroutine());
    }

    protected virtual IEnumerator MoveRocateCoroutine()
    {
        int readyToNextState = 0;
        StartAction("startMove");

        while (readyToNextState == 0)
        {
            Vector2 nowPos = transform.position;
            Vector2 direction = (targetRocate - nowPos).normalized;
            Vector2 nextPos = direction * moveSpeed * Time.deltaTime;

            float distanceOfPlayer = Vector2.Distance(transform.position, player.transform.position);
            float distanceOfStone = Vector2.Distance(transform.position, targetRocate);

            Debug.DrawLine(transform.position, player.transform.position, Color.red);
            Debug.DrawLine(transform.position, targetRocate, Color.red);

            if (distanceOfPlayer <= detectDistancePlayer) // �Ÿ��� 7 ���ϸ� �÷��̾ �����ߴٰ� ��
            {
                Debug.Log("�߰ݸ����ȯ");
                readyToNextState = 1; //�÷��̾� �߰ݸ��� ��ȯ
                break;
            }
            else if (distanceOfStone <= detectDistanceStone)
            {
                Debug.Log("���ݸ����ȯ");
                readyToNextState = 2; //������Ʈ ���ݸ��� ��ȯ
                targetPos = theStone.transform.position;
                Debug.Log("������Ʈ�� ����Ÿ������ ����");
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
                StopAction("dontStopMove");
                yield return new WaitForSeconds(0.001f);
                
                SetMonsterState(MonsterState.Chase);
                yield break;
                //break;

            case 2:
                StopAction("stopAll");
                yield return new WaitForSeconds(0.001f);
                SetMonsterState(MonsterState.Attack);
                yield break;
                //break;
        }
    }

    private void OnChase()
    {
        Debug.Log("ü�̽� ������ȯ");
        StartCoroutine(ChaseCoroutine());
    }

    protected virtual IEnumerator ChaseCoroutine()
    {
        int readyToNextState = 0;
        StartAction("startMove");

        while (readyToNextState == 0)
        {
            Vector2 nowPos = transform.position;
            Vector2 playerPos = player.transform.position;
            Vector2 direction = (playerPos - nowPos).normalized;
            Vector2 nextPos = direction * runSpeed * Time.deltaTime;

            float distanceOfPlayer = Vector2.Distance(transform.position, player.transform.position);

            Debug.DrawLine(transform.position, player.transform.position, Color.green);


            if (distanceOfPlayer > failDistancePlayer)
            {
                readyToNextState = 1; //�÷��̾� �߰� ����
                //�ٽ� ������Ʈ �̵� ���� ��ȯ 1
                break;
            }
            else if (distanceOfPlayer <= attackDistance)
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
                yield break;
                //break;

            case 2:
                StopAction("stopAll");
                yield return new WaitForSeconds(0.001f);
                SetMonsterState(MonsterState.Attack);
                yield break;
                //break;
        }
    }

    private void OnAttack()
    {
        Debug.Log("���� ������ȯ");
        StartCoroutine(AttackCoroutine());
    }
    protected virtual IEnumerator AttackCoroutine()
    {
        int readyToNextState = 0;
        float attackTimer = 0f;
        StartAction("startAttack");

        /*Vector2 attackDirection = (targetPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        Vector3 startAngle = weapon.transform.eulerAngles;
        weapon.transform.rotation = Quaternion.Euler(startAngle.x, startAngle.y, angle);*/

        while (attackTimer < 1f || readyToNextState == 2)
        {
            attackTimer += 0.1f;
            readyToNextState = 1;

            yield return null;
        }

        switch (readyToNextState)
        {
            case 1:
                StopAction("stopAttack");
                yield return new WaitForSeconds(0.001f);
                SetMonsterState(MonsterState.Chase);
                yield break;
                //break;
            case 2:
                StopAction("stopAttack");
                yield return new WaitForSeconds(0.001f);
                SetMonsterState(MonsterState.GetDamage);
                yield break;
                //break;
        }
    }

    private void OnDead()
    {
        Debug.Log("��� ������ȯ");
        StartCoroutine(DeadCoroutine());
    }
    protected virtual IEnumerator DeadCoroutine()
    {
        StopAction("stopAll");
        yield return new WaitForSeconds(0.001f);

        StartAction("startDead");
        yield return new WaitForSeconds(1f);

        Destroy(this);
        yield break;
    }

    public virtual void StopAction(string action)
    {
        switch (action)
        {
            case "stopSpawn":
                anim.SetInteger("StateNum", 0);
                break;
            case "dontStopMove":
                anim.SetInteger("StateNum", 0);
                rigid.velocity = Vector2.zero;
                break;
            case "stopAttack":
                anim.SetInteger("StateNum", 0);
                rigid.velocity = Vector2.zero;
                col.isTrigger = false;
                break;
            case "stopAll":
                anim.SetInteger("StateNum", 0);
                rigid.velocity = Vector2.zero;
                weapon.MoveSet(false);
                break;
        }
    }
    public virtual void StartAction(string action)
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
                weapon.MoveSet(false);
                break;
            case "startAttack":
                anim.SetInteger("StateNum", 3);
                weapon.AttackSet(true);
                break;
            case "startDead":
                anim.SetInteger("StateNum", 4);
                weapon.DeadSet(true);
                break;
        }
    }
}

