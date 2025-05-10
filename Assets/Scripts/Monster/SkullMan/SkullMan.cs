using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullMan : BasicMonster
{
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D col;
    [SerializeField] float rollingSpeed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        testPlayer = FindObjectOfType<TestPlayer>().gameObject;
        theStone = FindObjectOfType<TheStone>().gameObject;

        FirstSetting();

        SetMonsterState(MonsterState.Spawn);
        anim.SetBool("isSpawn", true);

    }

    public void FirstSetting()
    {
        name = "스컬맨";
        moveSpeed = 3;
        rollingSpeed = 9f;
        currentHp = 20;
        maxHp = 20;

        targetRocate = theStone.transform.position;
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
            Vector2 direction = (targetRocate - nowPos).normalized;

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

        rigid.velocity = Vector2.zero;
        anim.SetBool("isMove", false);
        SetMonsterState(MonsterState.Chase);
    }

    protected override IEnumerator ChaseCoroutine()
    {
        bool isAttackPlayer = false;


        bool isFailToChasePlayer = false;
        anim.SetBool("isMove", true);

        while (!isFailToChasePlayer)
        {
            Vector2 nowPos = transform.position;
            Vector2 playerPos = testPlayer.transform.position;
            Vector2 direction = (playerPos - nowPos).normalized;

            float distanceOfPlayer = Vector2.Distance(transform.position, testPlayer.transform.position);

            Debug.DrawLine(transform.position, testPlayer.transform.position, Color.green);


            if (distanceOfPlayer > 10f)
            {
                isFailToChasePlayer = true;
            }
            else
            {
                //rigid.velocity = direction * runSpeed;
            }
            yield return null;
        }
        SetMonsterState(MonsterState.MoveRocate);
        rigid.velocity = Vector2.zero;
        anim.SetBool("isMove", false);
    }

    protected override IEnumerator AttackCoroutine()
    {

        yield return null;
    }
}
