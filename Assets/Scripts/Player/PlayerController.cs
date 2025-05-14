using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerController : MonoBehaviour //IBattleEntity
{
    [Header("Component References")]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] public WeaponHandler weaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    protected PlayerAnimationHandler playerAnimationHandler;
    protected PlayerStats playerStats;
    protected ResourcesHandler resourcesHandler;
    protected HealthSystem healthSystem;

    private Rigidbody2D _rigidbody;
    private Camera _camera;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private bool isDead = false;

    // Blink ���� ����
    private bool isBlink = false;
    //private float blinkDistance = 5f;
    private float blinkCooldown = 2f;
    private float lastBlinkTime = -999f;
    private float blinkDuration = 0.4f;

    public Vector2 MovementDirection => movementDirection;
    public Vector2 LookDirection => lookDirection;
    public bool IsDead => isDead;
    public float BlinkCooldownRemaining => Mathf.Max(0, blinkCooldown - (Time.time - lastBlinkTime)); // ���� ��Ÿ�� ���
    public bool IsBlink => isBlink;
    private Animator _animator;

    //�ù�
    //public GameObject fallingSpearSkillPrefab;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerStats = GetComponent<PlayerStats>();
        resourcesHandler = GetComponent<ResourcesHandler>();
        _camera = Camera.main;
        healthSystem = HealthSystem.Instance;
        _animator = GetComponentInChildren<Animator>();

        if (weaponPrefab != null)
            weaponHandler = Instantiate(weaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>();
    }

    private void Update()
    {
        if (isDead) return; // ��� �� �Է� ����

        HandleInput(); // �Է� ó��
        RotateCharacter(); // ���� ȸ��
        UpdateAnimation(); // �ִϸ��̼� ������Ʈ
        AttackDelay(); // ���� �Է� ����

        if (Input.GetKeyDown(KeyCode.Space) && CanBlink())
            StartCoroutine(PerformBlink());

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    _animator.Play("Dead"); // ���� ȣ��
        //}

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    CastFallingSpearSkill();
        //}
    }

    //void CastFallingSpearSkill()
    //{
    //    Vector3 targetPos = transform.position;
    //    GameObject skill = Instantiate(fallingSpearSkillPrefab);
    //    skill.GetComponent<FallingSpears>().Initialize(targetPos);
    //}

    private bool CanBlink()
    {
        return !isDead && !isBlink && Time.time - lastBlinkTime >= blinkCooldown;
    }

    private void FixedUpdate()
    {
        if (!isDead)
            HandleMovement(); // ���� �̵� ó��
    }

    private void HandleInput() // �̵�, ����
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);

        if (lookDirection.magnitude < 0.9f)
            lookDirection = Vector2.zero;
        else
            lookDirection = lookDirection.normalized;

        isAttacking = Input.GetMouseButtonDown(0);
    }

    private void RotateCharacter() // ȸ��
    {
        if (lookDirection == Vector2.zero) return;

        float rotZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        if (characterRenderer != null)
            characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);

        weaponHandler?.Rotate(isLeft);
    }

    private void HandleMovement()
    {
        Vector2 velocity = movementDirection * playerStats.Speed;
        _rigidbody.velocity = velocity;

        //isAttacking = Input.GetMouseButtonDown(0);
    }

    private void UpdateAnimation()
    {
        if (playerAnimationHandler == null) return;

        bool isMoving = movementDirection.magnitude > 0.1f;
        playerAnimationHandler.SetMovement(isMoving);
    }

    private IEnumerator PerformBlink() // ����
    {
        isBlink = true;
        lastBlinkTime = Time.time;
        playerAnimationHandler?.SetBlink(true);

        // 1. ��ǥ ��ġ ���
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 blinkDirection = (mouseWorldPos - (Vector2)transform.position).normalized;
        float blinkDistance = 3f;

        // 2. �浹 �˻� (Raycast + OverlapCircle)
        RaycastHit2D hit = Physics2D.Raycast( // ������ �߻��� �浹ü�� �����ϴ� �ٽ� ���� �Լ�(�� �˰� �־�� �ϴ� ���� �ϳ��ε�)
            transform.position, // origin ���� ���� ��ġ (�÷��̾� �� ��ġ)
            blinkDirection, // direction ���� ���� (���콺 ����)
            blinkDistance, // distance �˻� �Ÿ� ������ 3f�� ����
            LayerMask.GetMask("Obstacle")); // �˻��� Layer

        if (hit.collider != null)
        {
            blinkDistance = hit.distance * 0.95f; // 5% ���� ���� Ȯ��
        }

        Vector2 targetPos = (Vector2)transform.position + (blinkDirection * blinkDistance);

        // 3. ���� �̵� �� ���� ����ȭ
        Physics2D.SyncTransforms(); // ���� ��ġ�� ���������� ���� �ݿ�
        // Unity�� �⺻������ ������ ���� �� Transform ��������� ���������� �ݿ��ϱ⶧����
        // �������� ����ȭ�ϸ� �����̵� �� ��� �浹 �˻簡 ���������ٰ� �Ѵ�.
        // Raycast : �̵� �� ��ֹ� ���� ���� Ȯ���ϰ�
        // OverlapCircle : �̵� �� �̼��� ���� ���� ���� ����.

        // 4. Rigidbody�� �̵� (���� ���� ����)
        if (_rigidbody != null)
        {
            _rigidbody.MovePosition(targetPos); // transform.position���� ���� �������� ȣȯ���� ���ٰ� �Ѵ�.
                                                // ������ SyncTransforms()�� ������ ���� �߻� ������.
        }
        else
        {
            transform.position = targetPos;
        }

        // 5. �̵� �� �ٽ� ���� ����ȭ
        Physics2D.SyncTransforms();

        yield return new WaitForSeconds(blinkDuration);

        // 6. �̵� �� �߰� �浹 �˻� (���� ����)
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(
            transform.position,
            0.4f,
            LayerMask.GetMask("Obstacle"));

        if (overlaps.Length > 0)
        {
            // �浹 �߻� �� ���� ����� ������ ��ġ�� ����
            Vector2 escapeDir = (transform.position - (Vector3)overlaps[0].ClosestPoint(transform.position)).normalized;
            transform.position += (Vector3)(escapeDir * 0.1f);
            Physics2D.SyncTransforms();
        }

        playerAnimationHandler?.SetBlink(false);
        isBlink = false;
    }

    private void AttackDelay()
    {
        if (weaponHandler ==  null) return;

        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    private void Attack()
    {
        if (lookDirection != Vector2.zero)
        {
            weaponHandler?.Attack();
        }
    }

    public void TakeDamage(int dmg)//IBattleEntity attacker, int dmg)
    {
        if (isDead) return;

        resourcesHandler.ChangeHealth(-dmg);
        Debug.Log($"�÷��̾ {dmg} ������ ����!");
        playerAnimationHandler?.PlayDamage();
        healthSystem?.UpdateGraphics();
    }

    public void Die()
    {
        isDead = true;
        _rigidbody.velocity = Vector2.zero;
        playerAnimationHandler.PlayDeath();

        StartCoroutine(HandleDeathAfterAnimation());
    }

    private IEnumerator HandleDeathAfterAnimation()
    {
        yield return new WaitForSeconds(2f); // ��� �ִϸ��̼� ���̸�ŭ ���

        DeathPanelController.Instance.Show();

        Destroy(gameObject); // 
    }
}
