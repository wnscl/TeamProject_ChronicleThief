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


public class MonsterAi : MonoBehaviour
{

    [SerializeField] MonsterAiState nowState;
    [SerializeField] MonsterAiState nextState;

    [Header("move")]
    [SerializeField] protected Vector2 targetPos;
    [SerializeField] protected float moveSpeed;

    [Header("stat")]
    [SerializeField] protected bool survive;
    [SerializeField] protected bool isAttacked;
    [SerializeField] protected string name;
    [SerializeField] protected int hp;
    [SerializeField] public int Hp { get { return hp; } }
    [SerializeField] protected int atk;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float chaseRange;

    [Header("basic field")]
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected Animator anim;
    [SerializeField] protected BoxCollider2D col;
    [SerializeField] protected GameObject player;
    [SerializeField] protected MonsterMeleeWeapon weapon;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
        weapon = GetComponentInChildren<MonsterMeleeWeapon>();
        targetPos = player.transform.position;
    }


    protected IEnumerator MonsterStateRepeater(MonsterAiState nextState)
    {
        StopAction("All");
        yield return new WaitForSeconds(0.001f);

        Debug.Log("스테이트머신 가동");
        while (survive)
        {
            Debug.Log("스테이트머신 루프 가동");
            nowState = nextState;
            switch (nowState)
            {
                case MonsterAiState.Idle:
                    StopAction("All");
                    yield return new WaitForSeconds(0.001f);

                    yield return StartCoroutine(Idle());
                    nextState = DecideNextState();
                    Debug.Log($"상태결정 {nextState}");
                    break;

                case MonsterAiState.Spawn:
                    yield return StartCoroutine(Spawn());

                    StopAction("All");
                    yield return new WaitForSeconds(0.001f);

                    nextState = DecideNextState();
                    Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.Chase:
                    StopAction("All");
                    yield return new WaitForSeconds(0.001f);

                    yield return StartCoroutine(Chase());
                    nextState = DecideNextState();
                    Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.Attack:
                    StopAction("All");
                    yield return new WaitForSeconds(0.001f);

                    yield return StartCoroutine(Attack());
                    nextState = DecideNextState();
                    Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.GetDamage:
                    StopAction("All");
                    yield return new WaitForSeconds(0.001f);

                    yield return StartCoroutine(GetDamage());
                    nextState = DecideNextState();
                    Debug.Log($"상태결정 {nowState}");
                    break;

                case MonsterAiState.Dead:
                    //StopAction("All");
                    //yield return new WaitForSeconds(0.001f);

                    yield return StartCoroutine(Dead());
                    nextState = DecideNextState();
                    Debug.Log($"상태결정 {nowState}");
                    break;
            }
        }
    }
    private MonsterAiState DecideNextState()
    {
        float distanceOfPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (hp <= 0)
        {
            nextState = MonsterAiState.Dead;
            return MonsterAiState.Dead;
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

        if (isAttacked)
        {
            nextState = MonsterAiState.GetDamage;
            return MonsterAiState.GetDamage;
        }

        return MonsterAiState.Idle;
    }




    protected virtual IEnumerator Idle()
    {
        yield return new WaitForSeconds(1f);

    }

    protected virtual IEnumerator Spawn()
    {
        StartAction("Spawn");
        yield return new WaitForSeconds(1f);
        yield break;

    }
    protected virtual IEnumerator Chase()
    {
        StartAction("Chase");
        bool isReadyToAttack = false;   

        while (!isReadyToAttack)
        {
            float distanceOfPlayer = Vector2.Distance(transform.position, player.transform.position);
            Vector2 playerPos = player.transform.position;
            Vector2 nowPos = transform.position;
            Vector2 direction = (playerPos - nowPos).normalized;
            Vector2 nextPos = direction * moveSpeed * Time.deltaTime;

            rigid.MovePosition(rigid.position + nextPos);

            yield return new WaitForFixedUpdate();

            if (distanceOfPlayer <= attackRange)
            {
                isReadyToAttack = true;
            }
            if (hp <= 0)
            {
                StopAction("All");
                yield break;
            }
        }
        Debug.Log("추격와일문 탈출");
        yield break;
    }

    protected virtual IEnumerator Attack()
    {
        StartAction("Attack");
        yield return new WaitForSeconds(1f);
        yield break;

    }

    protected virtual IEnumerator GetDamage()
    {
        yield return new WaitForSeconds(1f);

    }

    protected virtual IEnumerator Dead()
    {
        StartAction("Dead");
        yield return new WaitForSeconds(1f);

        Destroy(this);
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
            case "stopAttack":
                anim.SetInteger("StateNum", 0);
                rigid.velocity = Vector2.zero;
                col.isTrigger = false;
                break;
            case "All":
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
            case "Spawn":
                anim.SetInteger("StateNum", 10);
                break;
            case "Chase":
                anim.SetInteger("StateNum", 1);
                weapon.MoveSet(true);
                break;
            case "GetDamage":
                anim.SetInteger("StateNum", 2);
                weapon.MoveSet(false);
                break;
            case "Attack":
                anim.SetInteger("StateNum", 3);
                weapon.AttackSet(true);
                break;
            case "Dead":
                anim.SetInteger("StateNum", 4);
                weapon.DeadSet(true);
                break;
        }
    }
}
