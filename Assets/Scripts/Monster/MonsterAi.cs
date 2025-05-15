using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public enum MonsterAiState
{
    Idle,
    Spawn, 
    Chase, 
    Attack,
    GetDamage,
    Dead 
}

//protected int currentHp;
//protected int MaxHp;


public abstract class MonsterAi : MonoBehaviour, IBattleEntity
{

    [SerializeField] protected MonsterAiState nowState;
    [SerializeField] protected MonsterAiState nextState;
    [SerializeField] protected bool isSpawn;


    [Header("move")]
    [SerializeField] public Vector2 targetPos;
    [SerializeField] protected float moveSpeed;

    [Header("stat")]
    [SerializeField] protected bool survive;
    [SerializeField] protected bool isAttacked;
    [SerializeField] protected string name;
    [SerializeField] protected int hp;
    public int Hp { get { return hp; } }
    [SerializeField] protected int atk;
    public int Atk { get { return atk; } }
    [SerializeField] protected float attackRange;
    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected int mobGold;
    
    [Header("basic field")]
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected Animator anim;
    [SerializeField] protected BoxCollider2D col;
    [SerializeField] protected GameObject player;
    [SerializeField] protected ResourcesHandler playerResource;


    [SerializeField] protected MonsterMeleeWeapon weapon;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
        playerResource = FindObjectOfType<ResourcesHandler>();
        weapon = GetComponentInChildren<MonsterMeleeWeapon>();

    }

    protected virtual void LateUpdate()
    {
        if (survive)
        {
            LookPlayer();
        }
    }
    protected virtual void LookPlayer()
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

    protected IEnumerator MonsterStateRepeater(MonsterAiState nextState)
    {
        //Debug.Log("스테이트머신 가동");
        while (survive)
        {
            if (isSpawn)
            {
                yield return StartCoroutine(Idle());
                //Debug.Log($"아이들 작동");
            }
            nowState = nextState;

            switch (nowState)
            {
                case MonsterAiState.Spawn:
                    yield return StartCoroutine(Spawn());
                    nextState = DecideNextState();
                    //Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.Chase:
                    yield return StartCoroutine(Chase());
                    nextState = DecideNextState();
                    //Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.Attack:
                    yield return StartCoroutine(Attack());
                    nextState = DecideNextState();
                    //Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.GetDamage:
                    yield return StartCoroutine(GetDamage());
                    nextState = DecideNextState();
                    //Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.Dead:
                    yield return StartCoroutine(Dead());
                    nextState = DecideNextState();
                    //Debug.Log($"상태결정 {nowState}");
                    break;
            }
        }
    }
    protected virtual MonsterAiState DecideNextState()
    {
        float distanceOfPlayer = 
            Vector2.Distance(transform.position, player.transform.position);

        if (hp <= 0)
        {
            nextState = MonsterAiState.Dead;
            return MonsterAiState.Dead;
        }

        if (isAttacked)
        {
            nextState = MonsterAiState.GetDamage;
            return MonsterAiState.GetDamage;
        }

        if (distanceOfPlayer <= attackRange)
        {
            nextState = MonsterAiState.Attack;  
            return MonsterAiState.Attack;
        }

        if (distanceOfPlayer <= chaseRange)
        {
            nextState = MonsterAiState.Chase;
            return MonsterAiState.Chase;
        }

        return MonsterAiState.Chase;
    }

    protected virtual IEnumerator Idle()
    {
        StopAction("All");
        yield return new WaitForSeconds(0.0005f);
        yield break;    
    }

    protected virtual IEnumerator Spawn()
    {
        StartAction("Spawn");
        yield return new WaitForSeconds(1.02f);
        isSpawn = true; 
        yield break;

    }
    protected virtual IEnumerator Chase()
    {
        StartAction("Chase");
        bool isReadyToAttack = false;   

        while (!isReadyToAttack)
        {
            float distanceOfPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceOfPlayer <= attackRange)
            {
                isReadyToAttack = true;
            }
            if (isAttacked)
            {
                yield break;
            }

            Vector2 playerPos = player.transform.position;
            Vector2 nowPos = transform.position;
            Vector2 direction = (playerPos - nowPos).normalized;
            Vector2 nextPos = direction * moveSpeed * Time.deltaTime;

            rigid.MovePosition(rigid.position + nextPos);
            rigid.velocity = Vector2.zero;

            yield return new WaitForFixedUpdate();

        }
        //Debug.Log("추격와일문 탈출");
        yield break;
    }

    protected virtual IEnumerator Attack()
    {
        Vector2 nowPos = transform.position;
        float frameTimer = 0f;

        StartAction("Attack");

        while (frameTimer < attackDuration)
            //정해진 시간 동안 반복문을 사용
        {
            if (isAttacked)
            {
                yield break ;
            }

            transform.position = nowPos;
            rigid.velocity = Vector2.zero;

            frameTimer += Time.fixedDeltaTime;
            //Time.time은 게임이 시작된 후 흐른 총 시간
            //픽스드 업데이트 주기에 맞춤
            //time.deltaTime update주기에 맞춤
            yield return new WaitForFixedUpdate();
            //여기서는 픽스드업데이트 프레임 단위로 사용
        }

        yield break;
    }

    protected virtual IEnumerator GetDamage()
    {
        isAttacked = false;
        StartAction("GetDamage");
        yield return new WaitForSeconds(0.3f);
        yield break;
    }

    protected virtual IEnumerator Dead()
    {
        StartAction("Dead");
        playerResource.AddGold(mobGold);
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
        yield break;
    }


    public virtual void StopAction(string action)
    {
        switch (action)
        {
            case "dontStopMove":
                anim.SetInteger("StateNum", 0);
                rigid.velocity = Vector2.zero;
                break;
            case "All":
                anim.SetInteger("StateNum", 0);
                rigid.velocity = Vector2.zero;
                weapon.MoveAnimationSet(false);
                weapon.AttackAnimationSet(false);
                break;
        }
    }
    public virtual void StartAction(string action)
    {
        switch (action)
        {
            case "Spawn":
                anim.SetInteger("StateNum", 10);
                weapon.SpawnSetting();
                break;
            case "Chase":
                anim.SetInteger("StateNum", 1);
                weapon.MoveAnimationSet(true);
                break;
            case "GetDamage":
                anim.SetInteger("StateNum", 2);
                weapon.MoveAnimationSet(false);
                break;
            case "Attack":
                anim.SetInteger("StateNum", 3);
                weapon.AttackAnimationSet(true);
                break;
            case "Dead":
                anim.SetInteger("StateNum", 4);
                weapon.DeadAnimationSet(true);
                survive = false;
                break;
        }
    }

    public void TakeDamage(IBattleEntity attacker, int dmg)
    {
        if(!survive) { return; }    

        hp -= dmg;

        if (hp <= 0)
        {
            isAttacked = true;
        }
        else
        {
            isAttacked = true;
        }
    }
}
