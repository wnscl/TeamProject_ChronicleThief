using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    None,
    Spawn,
    Idle,
    MoveRocate,
    Chase,
    Attack,
    Dead
}

public class BasicMonster : MonoBehaviour
{
    private MonsterState CurrentState = MonsterState.Spawn;
    private MonsterState NextState = MonsterState.None;


    public TestPlayer player;
    
    public void SetMonsterState(MonsterState state)
    {
        CurrentState = state;

        switch (CurrentState)
        {
            case MonsterState.Spawn:
                OnSpawn();
                break;

            case MonsterState.Idle:

                break;

            case MonsterState.MoveRocate:

                break;

            case MonsterState.Chase:

                break;

            case MonsterState.Attack:

                break;

            case MonsterState.Dead:

                break;
        }
    }

    private void OnSpawn()
    {
        Debug.Log("���� ����");
        SetMonsterState(MonsterState.Idle);
    }

    private void OnIdle()
    {
        //idle �ִϸ��̼� ���
        //idle�� ���� �ؾ� �� �͵��� ó��
        SetMonsterState(MonsterState.MoveRocate);
    }

    private void OnMoveRocate()
    {
        StartCoroutine(MoveRocateCoroutine());
    }

    private IEnumerator MoveRocateCoroutine()
    {
        bool isDetectedPlayer = false;

        while (isDetectedPlayer == false)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceFromPlayer <= 3f) // �Ÿ��� 3 ���ϸ� �÷��̾ �����ߴٰ� ��
            {
                isDetectedPlayer = true;
            }
            else
            {
                Debug.Log("�÷��̾ �� ã�ҽ��ϴ�.");
            }

            yield return null;
            //���ؽ�Ʈ = �ƶ�
            //����Ƽ�� �������ӿ��� �ؾ��� �͵��� �پ��ѵ�
            //���Ϲ��� �ڵ尡 ���������� �ٸ� �͵��� ������ �ȵǱ� ������
            //�ڷ�ƾ�� ����Ͽ� �ϸ��� �������� �� ���Ϲ��� ����������
            //���������ӿ� �ٽ� ���Ϲ��� ������� ��
            //�� ���� �����ӿ� ������ �ɾ�� 
            // �׸��� �̰��� yield return null�� ���̺� ����Ʈ��� ����
            //--> �ڷ�ƾ ���� current��� ���� ���¸� ��Ƶδ� ������ �ְ�
            //�׷��� ������ ����� �� ���Ϲ��� ������ �ʾҴٸ�
            //Ŀ��Ʈ���� ��Ƶδ� ���� "yield return null���� ���������"
            //�� ������ ����ɶ��� ���̺�����Ʈ���� ����
        }

        SetMonsterState(MonsterState.Chase);
    }

}
