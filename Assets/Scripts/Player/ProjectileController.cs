using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private WeaponHandler weaponHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestroy = true;

    private IBattleEntity attacker; // 추가: 공격자 참조

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration += Time.deltaTime;

        if (currentDuration > weaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        _rigidbody.velocity = direction * weaponHandler.AttackSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 지형/벽 충돌 시 이펙트 생성 후 파괴
        if ((levelCollisionLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Vector2 hitPoint = collision.ClosestPoint(transform.position) - direction * 0.2f;
            DestroyProjectile(hitPoint, true); // FX 생성 활성화
            return; // 이후 로직 무시
        }
        // 2. 몬스터 충돌
        if ((weaponHandler.target.value & (1 << collision.gameObject.layer)) != 0)
        {
            IBattleEntity target = collision.GetComponent<IBattleEntity>();
            if (target != null)
            {
                BattleSystemManager.Instance.AttackOther(attacker, target);
            }
            Destroy(gameObject); // 몬스터와 충돌 시 파괴
        }
    }


    public void Init(Vector2 direction, WeaponHandler weaponHandlers, IBattleEntity attacker)
    {
        this.attacker = attacker;
        weaponHandler = weaponHandlers;
        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.WeaponSize;

        // 스프라이트가 -45° 기울어져 있으므로, +45°를 추가로 회전시켜 보정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 45); // +45°로 보정

        // pivot 방향 처리 (좌우 반전)
        if (direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, -90);

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        Destroy(this.gameObject);
    }
}
