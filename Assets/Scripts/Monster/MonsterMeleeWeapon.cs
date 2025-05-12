using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMeleeWeapon : MonoBehaviour
{
    Animator weaponAnim;
    BoxCollider2D weaponCol;

    private void Awake()
    {
        weaponAnim = GetComponentInChildren<Animator>();
        weaponCol = GetComponent<BoxCollider2D>();
    }

    //public void SettingMelee()
    //{
    //    Vector3 startAngle = this.transform.eulerAngles;

    //}


    public void MoveSet(bool isMoving)
    {
        if (isMoving)
        {
            weaponAnim.SetInteger("actionNum", 1);
        }
        else
        {
            weaponAnim.SetInteger("actionNum", 0);
        }
    }

    public void AttackSet(bool isAttack)
    {
        if (isAttack)
        {
            weaponAnim.SetInteger("actionNum", 3);
        }
        else
        {
            weaponAnim.SetInteger("actionNum", 0);
        }
    }
}
