using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullRunnerAnimEvent : MonoBehaviour
{
    MonsterMeleeWeapon mobWeapon;

    private void Awake()
    {
        mobWeapon = GetComponentInParent<MonsterMeleeWeapon>();
    }

    public void OnAttack()
    {
        Debug.Log("애니메이션 이벤트 실행");
        mobWeapon.AttackStart();
    }
    public void OnAttackEnd()
    {
        Debug.Log("애니메이션 이벤트 종료");
        mobWeapon.AttackEnd();
    }

}
