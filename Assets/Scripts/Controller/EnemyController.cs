using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStatus
    {
        Idle,
        Wreck,
        Battle
    }
    public EnemyStatus currentStatus;
    public enum TypeSounds
    {
        MRSA,
        SP,
        BC,
        VP,
        AS
    }
    public TypeSounds currentTypeSounds;
    private float M_MaxHp;                    //怪物最大血量
    private float M_CurrentHP;                //怪物現有血量
    private Image HP_Bar;                   //顯示怪物血量UI
    public float lookRadius = 10f;
    [SerializeField] private float RunSpeed;
    [SerializeField]private float AttackSpeed;

    public Transform target;
    private Vector3 origin_Pos;
    private NavMeshAgent agent;

    private Sounds      Sounds;
    public AudioSource  Monsteraudio;
    private Animator    EnemyAni;
    private GameObject  one_point;

    private bool        isAttackDegree;  //攻擊冷卻度
    private bool        isMove;          //判斷是否在移動
    [SerializeField]public bool isBattle;  //碰觸陷阱直接戰鬥

    void Awake()
    {
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        Monsteraudio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始化
        currentStatus = EnemyStatus.Wreck;
        isAttackDegree = false;
        isMove = false;
        isBattle = false;

        //抓取屬性
        HP_Bar = transform.Find("HPCanvas").Find("CurrentHP").GetComponent<Image>();
        origin_Pos = transform.position;
        agent = GetComponent<NavMeshAgent>();
        EnemyAni = transform.GetChild(0).GetComponent<Animator>();
        one_point = transform.GetChild(1).gameObject;
        one_point.SetActive(false);

        if (currentTypeSounds == TypeSounds.MRSA)
        {
            M_MaxHp = 3;
            RunSpeed = Random.Range(4f, 5.5f);
            AttackSpeed = Random.Range(0.8f, 1.2f);
        }
        if (currentTypeSounds == TypeSounds.SP)
        {
            M_MaxHp = 2;
            RunSpeed = Random.Range(5f, 7.5f);
            AttackSpeed = Random.Range(0.8f, 1.2f);
        }
        if (currentTypeSounds == TypeSounds.BC)
        {
            M_MaxHp = 3;
            RunSpeed = 4f;
            AttackSpeed = Random.Range(1f, 1.5f);
        }
        if (currentTypeSounds == TypeSounds.VP)
        {
            M_MaxHp = 1;
            RunSpeed = 3f;
            AttackSpeed = Random.Range(1.6f, 2f);
        }
        if (currentTypeSounds == TypeSounds.AS)
        {
            M_MaxHp = 10;
            RunSpeed = 4.8f;
            AttackSpeed = Random.Range(2.5f, 3f);
        }
        M_CurrentHP = M_MaxHp;                      //怪物血量初始化
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP:" + M_CurrentHP);
        if (Input.GetKeyDown(KeyCode.D))
        {
            MonsterHurt(1);
        }
        
        float distance = Vector3.Distance(target.position, transform.position);
        float origin_distance = Vector3.Distance(origin_Pos, transform.position);

        switch (currentStatus)
        {
            case EnemyStatus.Idle:
                //面向目標
                FaceTarget();
                if (distance <= lookRadius)
                {
                    currentStatus = EnemyStatus.Battle;
                }
                break;
            case EnemyStatus.Wreck:
                //攻擊肺動脈
                if (!isAttackDegree)
                {
                    isAttackDegree = true;
                    Invoke("Attack", AttackSpeed);
                }
                if (distance <= lookRadius || isBattle)
                {
                    one_point.SetActive(true);
                    isMove = true;
                    _Move();                        //怪物發生音效
                    Invoke("Close_onepoint", 1f);
                    currentStatus = EnemyStatus.Battle;
                }
                break;
            case EnemyStatus.Battle:
                if (isBattle)
                {
                    //觸發器觸發的攻擊模式
                    BattleT(distance, origin_distance);
                }
                else
                {
                    //距離觸發的攻擊模式
                    Battle(distance, origin_distance);
                }
                

                break;
        }
        
    }

    void Battle(float _distance,float _origin_distance)
    {
        if (_distance <= lookRadius)
        {
            //追擊狀態
            agent.SetDestination(target.position);
            agent.speed = RunSpeed;
            EnemyAni.SetBool("Run Forward", true);
            if (_distance <= agent.stoppingDistance)
            {
                //面向目標
                FaceTarget();
                EnemyAni.SetBool("Run Forward", false);
                //攻擊目標
                EnemyAni.SetTrigger("Attack 01");
            }
        }
        else
        {
            //逃脫狀態
            agent.SetDestination(origin_Pos);
            agent.speed = 3f;
            EnemyAni.SetBool("Run Forward", false);
            EnemyAni.SetBool("Walk Forward", true);
            if (_origin_distance <= agent.stoppingDistance)
            {
                EnemyAni.SetBool("Walk Forward", false);
                currentStatus = EnemyStatus.Idle;
            }
        }
    }

    void BattleT(float _distance, float _origin_distance)
    {
        //追擊狀態
        agent.SetDestination(target.position);
        agent.speed = RunSpeed;
        
        EnemyAni.SetBool("Run Forward", true);          //開啟移動動作

        if (_distance <= agent.stoppingDistance + 1f)
        {
            if (isMove)
            {
                isMove = false;
                CancelInvoke("_Move");
            }
        }
        if (_distance <= agent.stoppingDistance)
        {
            //面向目標
            FaceTarget();
            EnemyAni.SetBool("Run Forward", false);     //取消移動動作
            
            //攻擊目標
            if (!isAttackDegree)
            {
                isAttackDegree = true;
                Invoke("Attack", AttackSpeed);
            }
        }
    }

    public void MonsterHurt(int Damage)
    {
        if (M_CurrentHP > 0)
        {
            if (M_CurrentHP <= Damage)
            {
                _Die();
            }
            M_CurrentHP = M_CurrentHP - Damage;
            Debug.Log("C" + M_CurrentHP + "/" + "M" + M_MaxHp);
            HP_Bar.fillAmount = M_CurrentHP / M_MaxHp;
        }
    }

    void Attack()
    {
        Debug.Log("Attack");
        EnemyAni.SetTrigger("Attack 01");   //攻擊動作
        isAttackDegree = false;             //攻擊冷卻度完成
        //攻擊音效
        if (currentTypeSounds == TypeSounds.MRSA || currentTypeSounds == TypeSounds.SP)
        {
            Monsteraudio.clip = Sounds.Monster_Attack[2];
        }
        if (currentTypeSounds == TypeSounds.BC)
        {
            Monsteraudio.clip = Sounds.Monster_Attack[1];
        }
        if (currentTypeSounds == TypeSounds.VP)
        {
            Monsteraudio.clip = Sounds.Monster_Attack[0];
        }
        if (currentTypeSounds == TypeSounds.AS)
        {
            Monsteraudio.clip = Sounds.Monster_Attack[3];
        }

        if (currentStatus == EnemyStatus.Wreck)
        {
            Monsteraudio.volume = 0.6f;
        }
        if (currentStatus == EnemyStatus.Battle)
        {
            Monsteraudio.volume = 0.6f;
        }

        Monsteraudio.Play();
    }

    void Close_onepoint()
    {
        one_point.SetActive(false);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void _Move()
    {
        Debug.Log("MMM");
        //移動音效
        if (currentTypeSounds == TypeSounds.MRSA || currentTypeSounds == TypeSounds.SP)
        {
            Monsteraudio.clip = Sounds.Monster_Move[2];
            Monsteraudio.volume = 0.4f;
            Invoke("_Move", 2f);
        }
        if (currentTypeSounds == TypeSounds.BC)
        {
            Monsteraudio.clip = Sounds.Monster_Move[1];
            //Monsteraudio.minDistance = 6f;
            Monsteraudio.volume = 0.5f;
            Invoke("_Move", 1f);
        }
        if (currentTypeSounds == TypeSounds.VP)
        {
            Monsteraudio.clip = Sounds.Monster_Move[0];
            Monsteraudio.volume = 0.5f;
            Invoke("_Move", 0.8f);
        }
        if (currentTypeSounds == TypeSounds.AS)
        {
            Monsteraudio.clip = Sounds.Monster_Move[3];
            Monsteraudio.volume = 0.6f;
            Invoke("_Move", 4f);
        }
        Monsteraudio.Play();
    }
    void _Die()
    {
        agent.Stop();
        EnemyAni.SetTrigger("Die");
        CancelInvoke("Attack");
        CancelInvoke("_Move");
        //死掉音效
        if (currentTypeSounds == TypeSounds.MRSA || currentTypeSounds == TypeSounds.SP || currentTypeSounds == TypeSounds.BC || currentTypeSounds == TypeSounds.VP)
        {
            
            Monsteraudio.clip = Sounds.Monster_Die[0];
            Destroy(this.gameObject, 2f);
        }
        if (currentTypeSounds == TypeSounds.AS)
        {
            Monsteraudio.clip = Sounds.Monster_Die[1];
            Monsteraudio.volume = 0.4f;
            Destroy(this.gameObject, 3.5f);
        }
        Monsteraudio.Play();
    }
}
