using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BowManArrow : MonoBehaviour
{
    [SerializeField] BoxCollider2D col;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] RangeMonsterAi mob;
    [SerializeField] PlayerController player;
    [SerializeField] TheStone stone;

    [SerializeField] Vector2 targetPosition;
    [SerializeField] Vector2 direction;

    public void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        mob = GetComponentInParent<RangeMonsterAi>();
        player = FindObjectOfType<PlayerController>();
        stone = FindObjectOfType<TheStone>();

        if (mob.isTargetPlayer)
        {
            targetPosition = player.transform.position;
        }
        else
        {
            targetPosition = stone.transform.position;
        }
        
        Vector2 goPos = new Vector2(targetPosition.x, targetPosition.y);
        Vector2 nowPos = new Vector2(transform.position.x, transform.position.y);

        direction = (goPos - nowPos).normalized;

    }

    public void FixedUpdate()
    {
        rigid.velocity = direction * mob.arrowSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 지형/벽 충돌 시 이펙트 생성 후 파괴
        //if ((levelCollisionLayer.value & (1 << collision.gameObject.layer)) != 0)
        //{
        //    Vector2 hitPoint = collision.ClosestPoint(transform.position) - direction * 0.2f;
        //    DestroyProjectile(hitPoint, true); // FX 생성 활성화
        //    return; // 이후 로직 무시
        //}
        //// 2. 몬스터 충돌
        //if ((weaponHandler.target.value & (1 << collision.gameObject.layer)) != 0)
        //{
        //    IBattleEntity target = collision.GetComponent<IBattleEntity>();
        //    if (target != null)
        //    {
        //        BattleSystemManager.Instance.AttackOther(attacker, target);
        //    }
        //    Destroy(gameObject); // 몬스터와 충돌 시 파괴
        //}
        if (collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            //playerController = collision.gameObject.GetComponent<PlayerController>();
            BattleSystemManager.Instance.AttackPlayer(mob.Atk);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(this.gameObject);
        }
    }
}
