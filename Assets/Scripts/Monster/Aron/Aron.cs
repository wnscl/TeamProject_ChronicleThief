using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


enum AronState
{
    Chase,
    Attack,
    Dead
}

public class Aron : MonoBehaviour , IBattleEntity
{
    //창쓰는 첫번째보스
    //1. 위로 점프를 엄청 크게 한 다음 내려오면서 콱 찍음
    //2. 투명되고 잠시 대기 프리팹소환 프리팹은 창을 앞으로 내지르는 프리팹
    //3. 창이 존나게 떨어지는거 



    [Header("basic field")]
    [SerializeField] protected Rigidbody2D rigid;
    public Animator anim;
    public Animator weaponAnim;
    public SpriteRenderer weaponSprite;
    [SerializeField] protected BoxCollider2D col;
    [SerializeField] protected GameObject player;
    public GameObject weapon;
    //[SerializeField] protected GameObject weaponScrips;
    public GameObject fallingSpearSkillPrefab;
    public GameObject HeartAttackSkillPrefab;
    public GameObject attackPrefabs3;
    [SerializeField] AronState nowState;
    [SerializeField] AronState nextState;
    protected bool isSpawn;

    [Header("move")]
    public Vector2 playerPos;
    public Vector2 directionOfPlayer;
    public float distanceOfPlayer;
    [SerializeField] protected float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [Header("stat")]
    [SerializeField] protected bool survive;
    public bool Survive => survive;
    [SerializeField] protected bool isAttacked;
    [SerializeField] protected bool canAttack = false;
    [SerializeField] public string name;
    protected int hp;
    public int Hp { get { return hp; } }
    protected int atk;
    public int Atk { get { return atk; } }
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDuration;
    protected int SelectPrefabs = 0;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    protected virtual void Start()
    {
        AronFirstSetting();
        StartCoroutine(AronStateRepeater(AronState.Chase));
    }



    private void FixedUpdate()
    {
        if (!survive)
        {
            StartCoroutine(AronDead());
        }
        playerPos = player.transform.position;
        directionOfPlayer = (player.transform.position - transform.position).normalized;
        distanceOfPlayer = Vector2.Distance(transform.position, playerPos);

        if (survive)
        {
            LookPlayer();
        }

        if (nowState == AronState.Chase && survive)
        {
            AronMove();
        }

    }

    protected virtual void AronFirstSetting()
    {
        name = "아론";
        hp = 1500;
        atk = 95;
        survive = true;
        isAttacked = false;
        isSpawn = false;
        moveSpeed = 7;
        attackRange = 2f;
    }

    private void AronMove()
    {

        Vector2 nextPos = directionOfPlayer * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextPos);
        rigid.velocity = Vector2.zero;
        if (distanceOfPlayer <= attackRange)
        {
            canAttack = true;
        }
    }
    private void LookPlayer()
    {
        float weaponAngle = Mathf.Atan2(directionOfPlayer.y, directionOfPlayer.x) * Mathf.Rad2Deg;


        if (player != null)
        {
            if (transform.position.x > player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }
            //weapon.transform.rotation = Quaternion.Euler(0, 0, weaponAngle);
        }
    }

    IEnumerator AronStateRepeater(AronState nextState)
    {
        while (survive)
        {
            nowState = nextState;
            StopAction();
            yield return new WaitForSeconds(0.005f);

            switch (nowState)
            {
                case AronState.Chase:
                    StartAttackAnim("Move");
                    yield return new WaitForSeconds(0.005f);
                    while (survive)
                    {
                        if (canAttack)
                        {
                            break;
                        }

                        yield return null;
                    }
                    nextState = DecideNextAronState();
                    break;

                case AronState.Attack:
                    yield return StartCoroutine(AronAttack());
                    nextState = DecideNextAronState();
                    break;
                case AronState.Dead:
                    yield return StartCoroutine(AronDead());
                    nextState = DecideNextAronState();
                    break;
            }
            yield return null;
        }
    }
    private AronState DecideNextAronState()
    {
        if (hp <= 0 || !survive)
        {
            nextState = AronState.Dead;
            return AronState.Dead;
        }
        if (canAttack)
        {
            nextState = AronState.Attack;
            return AronState.Attack;
        }
        //if (nowState == AronState.Attack)
        //{
        //    nextState = AronState.Chase;
        //    return AronState.Chase;
        //}

        return AronState.Chase;
    }

    private IEnumerator AronDead()
    {
        anim.SetBool("AnyAnimEnd", true);
        weaponAnim.SetInteger("AttackNum", 0);
        weaponAnim.SetBool("AnyAnimEnd", true);
        yield return new WaitForSeconds(0.005f);
        anim.SetBool("AnyAnimEnd", false);
        weaponAnim.SetBool("AnyAnimEnd", false);
        yield return new WaitForSeconds(0.005f);

        anim.SetBool("isDead", true);
        weaponAnim.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }
    protected virtual IEnumerator AronAttack()
    {
        float aniTimer = 0f;
        float skillTimer = 0f;
        int choice = Random.Range(0, 10);
        string attackName = CheckAttackPattern(choice);

        StartAttackAnim(attackName);
        yield return new WaitForSeconds(0.005f);

        while (aniTimer < 1)
        {
            if (!survive)
            {
                yield break;
            }
            aniTimer += Time.deltaTime;
            rigid.velocity = Vector3.zero;
            yield return null;
        }

        if (SelectPrefabs == 1) Instantiate(fallingSpearSkillPrefab, playerPos, Quaternion.identity, this.transform);
        else if (SelectPrefabs == 2) Instantiate(HeartAttackSkillPrefab, playerPos, Quaternion.identity, this.transform);

        while (skillTimer < attackDuration)
        {
            if (!survive)
            {
                yield break;
            }
            skillTimer += Time.deltaTime;
            rigid.velocity = Vector3.zero;
            yield return null;
        }

        attackDuration = 0f;
        canAttack = false;
        yield break;
    }

    protected virtual string CheckAttackPattern(int choice)
    {
        if (choice < 5)
        {
            attackDuration = 5f;
            return "FallingSpear"; 
        }
        else
        {
            attackDuration = 5f;
            return "HeartAttack";
        }
    }

    protected virtual void StartAttackAnim(string attackName)
    {
        switch (attackName)
        {
            case "FallingSpear":
                anim.SetBool("AnyAnimEnd", false);
                weaponAnim.SetBool("AnyAnimEnd", false);
                anim.SetInteger("AttackNum", 1);
                weaponAnim.SetInteger("AttackNum", 1);
                SelectPrefabs = 1;
                break;

            case "HeartAttack":
                anim.SetBool("AnyAnimEnd", false);
                weaponAnim.SetBool("AnyAnimEnd", false);
                anim.SetInteger("AttackNum", 2);
                weaponAnim.SetInteger("AttackNum", 2);
                SelectPrefabs = 2;
                break;
            case "Move":
                anim.SetBool("AnyAnimEnd", false);
                weaponAnim.SetBool("AnyAnimEnd", false);
                anim.SetInteger("AttackNum", 10);
                weaponAnim.SetInteger("AttackNum", 10);
                break;
        }
    }

    private void StopAction()
    {
        anim.SetBool("AnyAnimEnd", true);
        weaponAnim.SetBool("AnyAnimEnd", true);
        anim.SetInteger("AttackNum", 0);
        weaponAnim.SetInteger("AttackNum", 0);
        SelectPrefabs = 0;
    }

    public void TakeDamage(IBattleEntity attacker, int dmg)
    {
        if (!survive) { return; }

        hp -= dmg;

        if (hp <= 0)
        {
            survive = false;
        }
        else
        {
            isAttacked = true;
        }
    }
}
