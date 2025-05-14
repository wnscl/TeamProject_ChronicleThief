using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public abstract class RangeMonsterAi : MonsterAi, IBattleEntity
{
    [SerializeField] protected MonsterRangeWeapon rangeWeapon;
    [SerializeField] protected TheStone stone;
    [SerializeField] protected Vector2 playerPos;
    [SerializeField] protected float distanceOfPlayer;
    [SerializeField] protected Vector2 spawnPoint;
    [SerializeField] protected Vector2 arrowPos;
    [SerializeField] public float arrowSpeed;
    public GameObject arrow;


    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
        rangeWeapon = GetComponentInChildren<MonsterRangeWeapon>();

    }

    protected override void LateUpdate()
    {
        playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        distanceOfPlayer = Vector2.Distance(transform.position, targetPos);

        if (survive)
        {
            LookPlayer();
        }

    }

    protected override void LookPlayer()
        //목표물을 보이는 로직으로 재정의
        //재정의하였기에 이름은 룩 플레이어로 상속받는 스크립트와 같음
    {

        if (player != null && stone != null)
        {
            if (transform.position.x > targetPos.x)
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

    protected override MonsterAiState DecideNextState()
    {
        float distanceOfPlayer = Vector2.Distance(transform.position, player.transform.position);

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

        if (nowState == MonsterAiState.Chase)    //targetPos == Vector2.zero)
        {
            nextState = MonsterAiState.Attack;
            return MonsterAiState.Attack;
        }

        if (nowState == MonsterAiState.Attack)   //targetPos != Vector2.zero)
        {
            nextState = MonsterAiState.Chase;
            return MonsterAiState.Chase;
        }

        return MonsterAiState.Chase;
    }


    protected override IEnumerator Chase()
    {
        targetPos = Vector2.zero;

        StartAction("Chase");
        float checkDistanceToSpawnPoint = Vector2.Distance(transform.position, spawnPoint);
        Vector2 nowPos = transform.position;
        Vector2 direction = (spawnPoint - nowPos).normalized;

        while (checkDistanceToSpawnPoint > 0.05f)
        {
            if (isAttacked)
            {
                yield break;
            }

            Vector2 nextPos = direction * moveSpeed * Time.fixedDeltaTime;
            //추후 델타타임으로 테스트

            rigid.MovePosition(rigid.position + nextPos);
            rigid.velocity = Vector2.zero;

            checkDistanceToSpawnPoint = Vector2.Distance(transform.position, spawnPoint);

            yield return new WaitForFixedUpdate();
        }

        if (isAttacked)
        {
            yield break;
        }
        yield break;
    }

    protected override IEnumerator Attack()
    {
        StartAction("Attack");
        float frameTimer = 0f;
        int chance = Random.Range(0, 10); 

        while (frameTimer < attackDuration)
        {
            if (isAttacked)
            {
                yield break;
            }

            Vector2 movePos = new Vector2( 
                (transform.position.x - 5f),
                transform.position.y 
                );
            Vector2 nowPos = transform.position;    
            Vector2 direction = (movePos - nowPos).normalized;
            Vector2 nextPos = direction * 0.02f * Time.fixedDeltaTime;

            rigid.MovePosition(rigid.position + nextPos);
            rigid.velocity = Vector2.zero;

            frameTimer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
        CreateArrow(chance);

        if (isAttacked)
        {
            yield break;
        }
        yield break;
    }


    public void CreateArrow(float chace)
    {
        if (chace < 6)
        {
            targetPos = player.transform.position;
            Vector2 arrowDirection = (player.transform.position - transform.position).normalized;
            float arrowAngle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
            Debug.Log($"화살 각도: {arrowAngle}");
            arrowPos = (Vector2)transform.position + (arrowDirection * 1.5f);
            Instantiate(arrow, arrowPos, Quaternion.Euler(0,0, arrowAngle), this.transform);
        }
        else
        {
            targetPos = stone.transform.position;
            Vector2 arrowDirection = (stone.transform.position - transform.position).normalized;
            float arrowAngle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
            Debug.Log($"화살 각도: {arrowAngle}");
            arrowPos = (Vector2)transform.position + (arrowDirection * 1.5f);
            Instantiate(arrow, arrowPos, Quaternion.Euler(0, 0, arrowAngle), this.transform);
        }
        arrowPos = Vector2.zero;
    }

    protected override IEnumerator Spawn()
    {
        StartAction("Spawn");
        yield return new WaitForSeconds(1.02f);
        isSpawn = true;
        rangeWeapon.ShowWeapon();
        yield break;

    }


    public override void StopAction(string action)
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
                rangeWeapon.MoveAnimationSet(false);
                rangeWeapon.AttackAnimationSet(false);
                break;
        }
    }
    public override void StartAction(string action)
    {
        switch (action)
        {
            case "Spawn":
                anim.SetInteger("StateNum", 10);
                rangeWeapon.SpawnSetting();
                break;
            case "Chase":
                anim.SetInteger("StateNum", 1);
                rangeWeapon.MoveAnimationSet(true);
                break;
            case "GetDamage":
                anim.SetInteger("StateNum", 2);
                rangeWeapon.MoveAnimationSet(false);
                break;
            case "Attack":
                anim.SetInteger("StateNum", 3);
                rangeWeapon.AttackAnimationSet(true);
                break;
            case "Dead":
                anim.SetInteger("StateNum", 4);
                rangeWeapon.DeadAnimationSet(true);
                survive = false;
                break;
        }
    }
}
