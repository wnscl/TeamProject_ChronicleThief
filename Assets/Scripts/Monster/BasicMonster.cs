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
        Debug.Log("몬스터 생성");
        SetMonsterState(MonsterState.Idle);
    }

    private void OnIdle()
    {
        //idle 애니메이션 재생
        //idle에 따라 해야 할 것들을 처리
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

        SetMonsterState(MonsterState.Chase);
    }

}
