using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullRunnerWeapon : MonoBehaviour
{
    Animator weaponAnim;
    BoxCollider2D weaponCol;

    private void Awake()
    {
        weaponAnim = GetComponentInChildren<Animator>();
        weaponCol = GetComponent<BoxCollider2D>();
    }

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


}
