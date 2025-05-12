using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] private float delay = 1f; // 공격 딜레이
    public float Delay { get =>  delay; set => delay = value; }

    [SerializeField] private int bulletIndex; // 총알 프리팹 인덱스
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get =>  weaponSize; set { weaponSize = value; } }
    // 화살
    [SerializeField] private float duration; // 화살이 살아있는 시간
    public float Duration { get { return duration; } }

    [SerializeField] private float attackSpeed = 1f; // 공격 속도
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }

    [SerializeField] private float spread; // 총알 퍼짐 각도 범위
    public float Spread { get { return spread; } }

    [SerializeField] private int perShot; // 총알 갯수
    public int PerShot { get { return perShot; } }

    [SerializeField] private float shotAngle; // 총알들 간 고정 각도 간격
    public float ShotAngle { get { return shotAngle; } }

    [SerializeField] private Transform projectileSpawnPosition;

    public LayerMask target;

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public PlayerController playerController {  get; private set; }

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ProjectileManager projectileManager;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    private void Start()
    {
        projectileManager = ProjectileManager.Instance;
    }

    public virtual void Attack()
    {
        float shotAngleSpace = shotAngle; // 총알들 간 고정 각도 간격
        int perShots = perShot; // 총알 갯수

        float minAngle = -(perShots / 2f) * shotAngleSpace;

        for (int i = 0; i < perShots; i++)
        {
            float angle = minAngle + shotAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;

            CreateShot(playerController.LookDirection.normalized, angle);
        }

        AttackAnim();
    }

    private void CreateShot(Vector2 _lookDirection, float angle)
    {
        Vector2 normalizedDir = _lookDirection.normalized; // 반드시 정규화
        Vector2 rotatedDir = RotateVector2(normalizedDir, angle);
        ProjectileManager.Instance.ShootBullet(this, projectileSpawnPosition.position, rotatedDir);
    }

    public void AttackAnim()
    {
        animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        //spriteRenderer.flipY = isLeft;
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }

}
