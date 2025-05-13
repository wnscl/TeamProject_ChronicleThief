using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMeleeWeapon : MonoBehaviour
{
    [SerializeField] Animator weaponAnim;
    [SerializeField] BoxCollider2D weaponCol;
    [SerializeField] MonsterAi mob;
    [SerializeField] PlayerController playerController;

    private void Awake()
    {
        weaponAnim = GetComponentInChildren<Animator>();
        weaponCol = GetComponentInParent<BoxCollider2D>();
        mob = GetComponentInParent<MonsterAi>();
    }

    public void SpawnSetting()
    {
        weaponCol.enabled = false;
    }
    public void AttackStart()
    {
        weaponCol.enabled = true;
    }
    public void AttackEnd()
    {
        weaponCol.enabled = false;
    }

    public void MoveAnimationSet(bool isMoving)
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
    public void AttackAnimationSet(bool isAttack)
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
    public void DeadAnimationSet(bool isHit)
    {
        if (isHit)
        {
            weaponAnim.SetInteger("actionNum", 2);
        }
        else
        {
            weaponAnim.SetInteger("actionNum", 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player")) )
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            BattleSystemManager.Instance.AttackOther(mob , playerController);
        }
    }
}
