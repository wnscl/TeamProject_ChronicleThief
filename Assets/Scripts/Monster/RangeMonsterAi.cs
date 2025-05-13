using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeMonsterAi : MonsterAi, IBattleEntity
{
    
    [SerializeField] TheStone stone;
    [SerializeField] Vector2 playerPos;
    [SerializeField] float distanceOfPlayer;
    [SerializeField] Vector2 spawnPoint;
    public GameObject arrow;


    private void Start()
    {
        spawnPoint = transform.position;
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
        //��ǥ���� ���̴� �������� ������
        //�������Ͽ��⿡ �̸��� �� �÷��̾�� ��ӹ޴� ��ũ��Ʈ�� ����
    {

        if (player != null && stone != null)
        {
            if (transform.position.x > targetPos.x)
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
            //���� ��ŸŸ������ �׽�Ʈ

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

        while (frameTimer < 1f)
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
        if (chance < 6)
        {
            targetPos = player.transform.position;
            //���Ÿ� ���� ������ �߰�
        }
        else
        {
            targetPos = stone.transform.position;
            //���Ÿ� ���� ������ �߰�
        }

        if (isAttacked)
        {
            yield break;
        }
        yield break;
    }
}
