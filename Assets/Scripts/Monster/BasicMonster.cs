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
    private MonsterState NextState = MonsterState.None;

    [Header("stat")]
    [SerializeField] protected string name;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int maxHp;

    [Header("move")]
    [SerializeField] protected Vector2 targetRocate;


    protected GameObject testPlayer;
    protected GameObject theStone;

    public void SetMonsterState(MonsterState state)
    {
        CurrentState = state;

        switch (CurrentState)
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
        }
    }

    private void OnSpawn()
    {
        Debug.Log("스컬러너 생성");
        StartCoroutine(SpawnCoroutine());
    }
    protected virtual IEnumerator SpawnCoroutine()
    {
        yield return null;
    }

    private void OnGetDamage()
    {
        //idle 애니메이션 재생
        //idle에 따라 해야 할 것들을 처리
        Debug.Log("스컬러너 데미지 상태전환");
        StartCoroutine(GetDamageCoroutine());
    }
    protected virtual IEnumerator GetDamageCoroutine()
    {
        yield return null;
    }

    private void OnMoveRocate()
    {
        Debug.Log("스컬러너 무브로케이트 상태전환");
        StartCoroutine(MoveRocateCoroutine());
    }

    protected virtual IEnumerator MoveRocateCoroutine()
    {
        yield return null;
    }

    private void OnChase()
    {
        Debug.Log("스컬러너 체이스 상태전환");
        StartCoroutine(ChaseCoroutine());
    }

    protected virtual IEnumerator ChaseCoroutine()
    {
        yield return null;
    }

    private void OnAttack()
    {
        Debug.Log("스컬러너 어택 상태전환");
        StartCoroutine(AttackCoroutine());
    }
    protected virtual IEnumerator AttackCoroutine()
    {
        yield return null;
    }

    private void OnDead()
    {
        Debug.Log("스컬러너 사망 상태전환");
        StartCoroutine(DeadCoroutine());
    }
    protected virtual IEnumerator DeadCoroutine()
    {
        yield return null;
    }

}

