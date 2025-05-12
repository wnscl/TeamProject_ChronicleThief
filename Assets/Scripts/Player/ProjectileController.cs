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

    private IBattleEntity attacker; // �߰�: ������ ����

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
        // 1. ����/�� �浹 �� ����Ʈ ���� �� �ı�
        if ((levelCollisionLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Vector2 hitPoint = collision.ClosestPoint(transform.position) - direction * 0.2f;
            DestroyProjectile(hitPoint, true); // FX ���� Ȱ��ȭ
            return; // ���� ���� ����
        }
        // 2. ���� �浹
        if ((weaponHandler.target.value & (1 << collision.gameObject.layer)) != 0)
        {
            IBattleEntity target = collision.GetComponent<IBattleEntity>();
            if (target != null)
            {
                BattleSystemManager.Instance.AttackOther(attacker, target);
            }
            Destroy(gameObject); // ���Ϳ� �浹 �� �ı�
        }
    }


    public void Init(Vector2 direction, WeaponHandler weaponHandlers, IBattleEntity attacker)
    {
        this.attacker = attacker;
        weaponHandler = weaponHandlers;
        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.WeaponSize;

        // ��������Ʈ�� -45�� ������ �����Ƿ�, +45�Ƹ� �߰��� ȸ������ ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 45); // +45�Ʒ� ����

        // pivot ���� ó�� (�¿� ����)
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
