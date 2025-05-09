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

            if (distanceOfPlayer <= 5f) // 거리가 5 이하면 플레이어를 감지했다고 함
            {
                isDetectedPlayer = true;
            }
            else
            {
                rigid.velocity = direction * moveSpeed;
                //a = 이오브젝트 b = 타겟로케이트 b에서 a를 빼줘야함 그리고 노멀라이즈
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