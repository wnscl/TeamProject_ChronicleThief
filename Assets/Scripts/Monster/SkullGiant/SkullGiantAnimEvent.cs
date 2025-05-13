using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullGiantAnimEvent : MonoBehaviour
{
    MonsterMeleeWeapon mobWeapon;

    private void Awake()
    {
        mobWeapon = GetComponentInParent<MonsterMeleeWeapon>();
    }

    public void OnAttack()
    {
        Debug.Log("�ִϸ��̼� �̺�Ʈ ����");
        mobWeapon.AttackStart();
    }
    public void OnAttackEnd()
    {
        Debug.Log("�ִϸ��̼� �̺�Ʈ ����");
        mobWeapon.AttackEnd();
    }
}
