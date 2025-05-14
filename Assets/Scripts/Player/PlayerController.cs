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

    // Blink 관련 변수
    private bool isBlink = false;
    //private float blinkDistance = 5f;
    private float blinkCooldown = 2f;
    private float lastBlinkTime = -999f;
    private float blinkDuration = 0.4f;

    public Vector2 MovementDirection => movementDirection;
    public Vector2 LookDirection => lookDirection;
    public bool IsDead => isDead;
    public float BlinkCooldownRemaining => Mathf.Max(0, blinkCooldown - (Time.time - lastBlinkTime)); // 남은 쿨타임 계산
    public bool IsBlink => isBlink;
    private Animator _animator;

    //시범
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
        if (isDead) return; // 사망 시 입력 무시

        HandleInput(); // 입력 처리
        RotateCharacter(); // 무기 회전
        UpdateAnimation(); // 애니메이션 업데이트
        AttackDelay(); // 공격 입력 관리

        if (Input.GetKeyDown(KeyCode.Space) && CanBlink())
            StartCoroutine(PerformBlink());

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    _animator.Play("Dead"); // 강제 호출
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
            HandleMovement(); // 물리 이동 처리
    }

    private void HandleInput() // 이동, 공격
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

    private void RotateCharacter() // 회전
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

    private IEnumerator PerformBlink() // 점멸
    {
        isBlink = true;
        lastBlinkTime = Time.time;
        playerAnimationHandler?.SetBlink(true);

        // 1. 목표 위치 계산
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 blinkDirection = (mouseWorldPos - (Vector2)transform.position).normalized;
        float blinkDistance = 3f;

        // 2. 충돌 검사 (Raycast + OverlapCircle)
        RaycastHit2D hit = Physics2D.Raycast( // 광선을 발사해 충돌체를 감지하는 핵심 물리 함수(꼭 알고 있어야 하는 것중 하나인듯)
            transform.position, // origin 광선 시작 위치 (플레이어 현 위치)
            blinkDirection, // direction 광선 방향 (마우스 방향)
            blinkDistance, // distance 검사 거리 위에서 3f로 설정
            LayerMask.GetMask("Obstacle")); // 검사할 Layer

        if (hit.collider != null)
        {
            blinkDistance = hit.distance * 0.95f; // 5% 여유 공간 확보
        }

        Vector2 targetPos = (Vector2)transform.position + (blinkDirection * blinkDistance);

        // 3. 실제 이동 전 물리 동기화
        Physics2D.SyncTransforms(); // 현재 위치를 물리엔진에 강제 반영
        // Unity는 기본적으로 프레임 종료 시 Transform 변경사항을 물리엔진에 반영하기때문에
        // 수동으로 동기화하면 순간이동 후 즉시 충돌 검사가 가능해진다고 한다.
        // Raycast : 이동 전 장애물 존재 여부 확인하고
        // OverlapCircle : 이동 후 미세한 끼임 현상 보정 해줌.

        // 4. Rigidbody로 이동 (물리 엔진 적용)
        if (_rigidbody != null)
        {
            _rigidbody.MovePosition(targetPos); // transform.position보다 물리 엔진과의 호환성이 좋다고 한다.
                                                // 하지만 SyncTransforms()가 없으면 문제 발생 가능함.
        }
        else
        {
            transform.position = targetPos;
        }

        // 5. 이동 후 다시 물리 동기화
        Physics2D.SyncTransforms();

        yield return new WaitForSeconds(blinkDuration);

        // 6. 이동 후 추가 충돌 검사 (끼임 방지)
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(
            transform.position,
            0.4f,
            LayerMask.GetMask("Obstacle"));

        if (overlaps.Length > 0)
        {
            // 충돌 발생 시 가장 가까운 안전한 위치로 조정
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
        Debug.Log($"플레이어가 {dmg} 데미지 받음!");
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
        yield return new WaitForSeconds(2f); // 사망 애니메이션 길이만큼 대기

        DeathPanelController.Instance.Show();

        Destroy(gameObject); // 
    }
}
