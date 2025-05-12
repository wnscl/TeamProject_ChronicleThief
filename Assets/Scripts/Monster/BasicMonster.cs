using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum MonsterState
{
    None,
    Spawn, // 생성 됫을 때 상태 (초기화)
    GetDamage, // 플레이어가 공격할 텀을 만들어주는 상태 몬스터에 따라 다름
    MoveRocate, //정해진 목표지점으로 이동 그러나 이동 중 플레이어 발견 시 체이스로 상태 전환
    Chase, //플레이어를 발견해 쫒아오는 패턴
    Attack, //공격 후 휴식패턴 전환 몬스터에 따라 다름
    Dead // 사망시 모든 행동 중지 
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
                //transform.localScale = new Vector3(-1, 1, 1); // 왼쪽 보기
                //로컬스케일로도 구현가능
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                //transform.localScale = new Vector3(1, 1, 1); // 오른쪽 보기
                //로컬스케일로도 구현가능
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
        Debug.Log("생성 상태전환");
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
        //idle 애니메이션 재생
        //idle에 따라 해야 할 것들을 처리
        //Debug.Log("스컬러너 데미지 상태전환");
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
        Debug.Log("무브로케이트 상태전환");
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

            if (distanceOfPlayer <= detectDistancePlayer) // 거리가 7 이하면 플레이어를 감지했다고 함
            {
                Debug.Log("추격모드전환");
                readyToNextState = 1; //플레이어 추격모드로 전환
                break;
            }
            else if (distanceOfStone <= detectDistanceStone)
            {
                Debug.Log("공격모드전환");
                readyToNextState = 2; //오브젝트 공격모드로 전환
                targetPos = theStone.transform.position;
                Debug.Log("오브젝트를 공격타겟으로 지정");
                break;
            }
            else
            {
                //rigid.velocity = direction * moveSpeed;
                //a = 이오브젝트 b = 타겟로케이트 b에서 a를 빼줘야함 그리고 노멀라이즈
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
        Debug.Log("체이스 상태전환");
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
                readyToNextState = 1; //플레이어 추격 실패
                //다시 오브젝트 이동 모드로 전환 1
                break;
            }
            else if (distanceOfPlayer <= attackDistance)
            {
                readyToNextState = 2; //플레이어 공격범위 체크
                //공격모드로 전환 2
                targetPos = playerPos;
                Debug.Log("플레이어를 공격타겟으로 지정");
                break;
            }
            else
            {
                //rigid.velocity = direction * runSpeed;
                rigid.MovePosition(rigid.position + nextPos);
                rigid.velocity = Vector2.zero;
                //플레이어 추격 중
            }

            //yield return null;
            yield return new WaitForFixedUpdate();
            //MovePosition같은 물리 연산은
            //픽스드업데이트에서 하는 것이 맞다.
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
        Debug.Log("어택 상태전환");
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
        Debug.Log("사망 상태전환");
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

