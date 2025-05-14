using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangeWeapon : MonoBehaviour
{
    [SerializeField] Animator weaponAnim;
    [SerializeField] RangeMonsterAi mob;
    [SerializeField] PlayerController playerController;
    [SerializeField] SpriteRenderer sprite;

    private void Awake()
    {
        weaponAnim = GetComponent<Animator>();
        mob = GetComponentInParent<RangeMonsterAi>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SpawnSetting()
    {
        sprite.enabled = false;
    }
    public void ShowWeapon()
    {
        sprite.enabled = true;
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
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player")))
    //    {
    //        BattleSystemManager.Instance.AttackPlayer(mob.Atk);
    //    }
    //}
}
