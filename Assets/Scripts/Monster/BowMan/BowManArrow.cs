using System.Collections;
using System.Collections.Generic;
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

}
