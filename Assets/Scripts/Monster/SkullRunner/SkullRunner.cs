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
        //생성되는 스컬러너는 같은 애니메이터를 공유하기 때문에
        //스폰되는 시점이 다르더라도 목표를 쫒는 상황일 때 애니메이션이 동시점에 실행되어
        //몬스터가 겹쳤을 때 정말 한마리처럼 보이는 현상을 해결하기 위해
        //각 몬스터가 생성될 때 마다 start에서 애니메이션을 연결하는 허브인 아이들을
        //시작타이밍이 어긋나게해서 겹쳤을 때 여러마리처럼 보이게 하는 방식
    }

    public void FirstSetting()
    {
        name = "스컬러너";
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
                readyToNextState = 1; //플레이어 추격 실패
                //다시 오브젝트 이동 모드로 전환 1
                break;
            }
            else if (distanceOfPlayer <= 2f)
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
        //몬스터가 목표지점을 바라보는 방향값
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        //각도를 라디안으로 구하고 라디안을 디그리(도단위)로 변한

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
                readyToNextState = 1; //공격 성공
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

    if (distanceFromPlayer <= 3f) // 거리가 3 이하면 플레이어를 감지했다고 함
    {
        isDetectedPlayer = true;
    }
    else
    {
        Debug.Log("플레이어를 못 찾았습니다.");
    }

    yield return null;
    //콘텍스트 = 맥락
    //유니티는 한프레임에서 해야할 것들이 다양한데
    //와일문에 코드가 갇혀버리면 다른 것들은 실행이 안되기 때문에
    //코루틴을 사용하여 일리드 리턴으로 이 와일문을 빠져나가서
    //다음프레임에 다시 와일문을 돌리라는 것
    //즉 다음 프레임에 예약을 걸어둠 
    // 그리고 이것은 yield return null이 세이브 포인트라고 생각
    //--> 코루틴 내에 current라는 현재 상태를 담아두는 변수가 있고
    //그래서 다음에 실행될 때 와일문이 끝나지 않았다면
    //커런트에서 담아두는 것은 "yield return null까지 실행됬었다"
    //즉 다음에 실행될때는 세이브포인트부터 실행
}

SetMonsterState(MonsterState.Chase);*/