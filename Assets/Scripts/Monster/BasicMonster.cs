using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum MonsterState
{
    None,
    Spawn, // ���� ���� �� ���� (�ʱ�ȭ)
    GetDamage, // �÷��̾ ������ ���� ������ִ� ���� ���Ϳ� ���� �ٸ�
    MoveRocate, //������ ��ǥ�������� �̵� �׷��� �̵� �� �÷��̾� �߰� �� ü�̽��� ���� ��ȯ
    Chase, //�÷��̾ �߰��� �i�ƿ��� ����
    Attack, //���� �� �޽����� ��ȯ ���Ϳ� ���� �ٸ�
    Dead // ����� ��� �ൿ ���� 
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
        Debug.Log("���÷��� ����");
        StartCoroutine(SpawnCoroutine());
    }
    protected virtual IEnumerator SpawnCoroutine()
    {
        yield return null;
    }

    private void OnGetDamage()
    {
        //idle �ִϸ��̼� ���
        //idle�� ���� �ؾ� �� �͵��� ó��
        Debug.Log("���÷��� ������ ������ȯ");
        StartCoroutine(GetDamageCoroutine());
    }
    protected virtual IEnumerator GetDamageCoroutine()
    {
        yield return null;
    }

    private void OnMoveRocate()
    {
        Debug.Log("���÷��� ���������Ʈ ������ȯ");
        StartCoroutine(MoveRocateCoroutine());
    }

    protected virtual IEnumerator MoveRocateCoroutine()
    {
        yield return null;
    }

    private void OnChase()
    {
        Debug.Log("���÷��� ü�̽� ������ȯ");
        StartCoroutine(ChaseCoroutine());
    }

    protected virtual IEnumerator ChaseCoroutine()
    {
        yield return null;
    }

    private void OnAttack()
    {
        Debug.Log("���÷��� ���� ������ȯ");
        StartCoroutine(AttackCoroutine());
    }
    protected virtual IEnumerator AttackCoroutine()
    {
        yield return null;
    }

    private void OnDead()
    {
        Debug.Log("���÷��� ��� ������ȯ");
        StartCoroutine(DeadCoroutine());
    }
    protected virtual IEnumerator DeadCoroutine()
    {
        yield return null;
    }

}

