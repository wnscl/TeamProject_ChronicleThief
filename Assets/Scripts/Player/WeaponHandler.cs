using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private float delay = 1f; // ���� ������
    public float Delay { get =>  delay; set => delay = value; }

    [SerializeField] private int bulletIndex; // �Ѿ� ������ �ε���
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get =>  weaponSize; set { weaponSize = value; } }

    // ȭ��
    [SerializeField] private float duration; // ȭ���� ����ִ� �ð�
    public float Duration { get { return duration; } }

    [SerializeField] private float attackSpeed = 1f; // ���� �ӵ�
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }

    [SerializeField] private float spread; // �Ѿ� ���� ���� ����
    public float Spread { get { return spread; } }

    [SerializeField] private int perShot; // �Ѿ� ����
    public int PerShot { get { return perShot; } }

    [SerializeField] private float shotAngle; // �Ѿ˵� �� ���� ���� ����
    public float ShotAngle { get { return shotAngle; } }

    [SerializeField] private Transform projectileSpawnPosition;

    public LayerMask target;

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public PlayerController playerController {  get; private set; }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    public virtual void Attack()
    {
        float shotAngleSpace = shotAngle; // �Ѿ˵� �� ���� ���� ����
        int perShots = perShot; // �Ѿ� ����

        float minAngle = -(perShots / 2f) * shotAngleSpace;

        for (int i = 0; i < perShots; i++)
        {
            float angle = minAngle + shotAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;

            CreateShot(playerController.LookDirection, angle);
        }

        AttackAnim();
    }

    private void CreateShot(Vector2 _lookDirection, float angle)
    {

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
