using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum AronState
{
    Chase,
    Attack,
    Dead
}

public class Aron : MonoBehaviour , IBattleEntity
{
    //â���� ù��°����
    //1. ���� ������ ��û ũ�� �� ���� �������鼭 �� ����
    //2. ����ǰ� ��� ��� �����ռ�ȯ �������� â�� ������ �������� ������
    //3. â�� ������ �������°� 



    [Header("basic field")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator anim;
    [SerializeField] BoxCollider2D col;
    [SerializeField] GameObject player;
    [SerializeField] GameObject weapon;
    //[SerializeField] protected GameObject weaponScrips;
    public GameObject attackPrefabs1;
    public GameObject attackPrefabs2;
    public GameObject attackPrefabs3;
    [SerializeField] AronState nowState;
    [SerializeField] AronState nextState;
    [SerializeField] bool isSpawn;

    [Header("move")]
    public Vector2 playerPos;
    public Vector2 directionOfPlayer;
    public float distanceOfPlayer;
    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [Header("stat")]
    [SerializeField] private bool survive;
    public bool Survive => survive;
    [SerializeField] private bool isAttacked;
    [SerializeField] public string name;
    [SerializeField] private int hp;
    public int Hp { get { return hp; } }
    [SerializeField] private int atk;
    public int Atk { get { return atk; } }
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDuration;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        playerPos = player.transform.position;
        directionOfPlayer = (player.transform.position - transform.position).normalized;
        distanceOfPlayer = Vector2.Distance(transform.position, playerPos);

        if (survive)
        {
            LookPlayer();
        }


    }

    private void AronFirstSetting()
    {

    }


    private void LookPlayer()
    {
        if (player != null)
        {
            if (transform.position.x > player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                //���Ⱑ �÷��̾ ���ϰ�
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                //���Ⱑ �÷��̾ ���ϰ�
            }
        }
    }

    IEnumerator AronStateRepeater(AronState nextState)
    {
        while (survive)
        {
            nowState = nextState;

            switch (nowState)
            {
                case AronState.Chase:
                    //yield return StartCoroutine(Chase());
                    nextState = DecideNextAronState();
                    //Debug.Log($"���°��� {nowState}");
                    break;

                case AronState.Attack:
                    //yield return StartCoroutine(Attack());
                    nextState = DecideNextAronState();
                    //Debug.Log($"���°��� {nowState}");
                    break;
                case AronState.Dead:
                    //yield return StartCoroutine(Dead());
                    nextState = DecideNextAronState();
                    //Debug.Log($"���°��� {nowState}");
                    break;
            }
            yield return null;
        }
    }
    private AronState DecideNextAronState()
    {

/*        if (hp <= 0)
        {
            nextState = MonsterAiState.Dead;
            return MonsterAiState.Dead;
        }

        if (isAttacked)
        {
            nextState = MonsterAiState.GetDamage;
            return MonsterAiState.GetDamage;
        }

        if (distanceOfPlayer <= attackRange)
        {
            nextState = MonsterAiState.Attack;
            return MonsterAiState.Attack;
        }

        if (distanceOfPlayer <= chaseRange)
        {
            nextState = MonsterAiState.Chase;
            return MonsterAiState.Chase;
        }
*/
        return AronState.Chase;
    }

    private IEnumerator AronAttack()
    {
        int choice = Random.Range(0, 10);
        string attackName = CheckAttackPattern(choice);
        StartAttackAnim(attackName);



        while (!survive)
        {
            //�� �ִϸ��̼��� ����Ǵ� �ð���ŭ ���ݽð�
            yield return null;
        }


        //���������ջ���

        attackDuration = 0f;
        yield break;
    }

    private string CheckAttackPattern(int choice)
    {
        if (choice < 4)
        {
            attackDuration = 5f;
            return "SpearStrike"; 
        }
        else if (choice >= 4 && choice < 7)
        {
            attackDuration = 5f;
            return "HeartAttack";
        }
        else
        {
            attackDuration = 5f;
            return "SpearRain";
        }
    }

    private void StartAttackAnim(string attackName)
    {
        switch (attackName)
        {
            case "SpearStrike":
                
                break;

            case "HeartAttack":

                break;

            case "SpearRain":

                break;
        }
    }

























    public void TakeDamage(IBattleEntity attacker, int dmg)
    {
        if (!survive) { return; }

        hp -= dmg;

        if (hp <= 0)
        {
            isAttacked = true;
        }
        else
        {
            isAttacked = true;
        }
    }
}
