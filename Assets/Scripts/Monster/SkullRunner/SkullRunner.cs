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
        //생성되는 스컬러너는 같은 애니메이터를 공유하기 때문에
        //스폰되는 시점이 다르더라도 목표를 쫒는 상황일 때 애니메이션이 동시점에 실행되어
        //몬스터가 겹쳤을 때 정말 한마리처럼 보이는 현상을 해결하기 위해
        //각 몬스터가 생성될 때 마다 start에서 애니메이션을 연결하는 허브인 아이들을
        //시작타이밍이 어긋나게해서 겹쳤을 때 여러마리처럼 보이게 하는 방식

    }

    public void FirstSetting()
    {
        survive = true;
        isAttacked = false;
        isSpawn = false;
        name = "스컬러너";
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