using DG.Tweening;
using System.Collections;
using UnityEngine;
using Valve.VR;

public class Teach_WhiteBloodCell : MonoBehaviour
{
    //白血球狀態(戰鬥對戰綠膿桿菌、教使用者用抗生素槍)
    public enum State
    {
        Battle,
        TeachArm
    }
    public  State           currentState;               //目前白血球狀態

    [SerializeField]private int             ChatNum = 0;//聊天圖片編號
    private ChatController  ChatController;             //聊天腳本
    private GameObject      NPC_Chat;                   //白血球聊天物件
    private TriggerController TriggerController;        //控制器

    private Sounds          Sounds;                      //音效控制
    private Animator        WhiteAni;                   //白血球動畫控制

    public  Transform[]     pos;                           //白血球走路軌跡位置
    [Range(1.0f, 20.0f), SerializeField]
    private float           _moveDuration = 6.0f;    //DoMove時間
    public  GameObject    Enemy;                      //怪物目標(綠膿桿菌)物件
    private GameObject   Player;                       //玩家物件
    public  Transform       theDest;                    //抓物件位置
    private Transform       target;                      //目標位置

    public GameObject       Antibiotic_Gun;             //抗生素槍物件
    private bool isChase = true;         //追擊呼喊
    [SerializeField] private bool            isChat      = true;         //聊天
    [SerializeField] private bool            isBattleEnd = true;         //判斷戰鬥是否結束
    [SerializeField] public  bool            isGetArms   = true;         //判斷是否得到武器
    
    // Start is called before the first frame update
    void Awake()
    {
        //Enemy = GameObject.Find("Enemy").transform.Find("SlimePBR").gameObject;
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        WhiteAni = GetComponent<Animator>();
        TriggerController = GameObject.Find("Player").GetComponent<TriggerController>();
    }

    void Start()
    {
        //初始化
        isChase         = true;
        isChat          = true;
        isBattleEnd     = false;
        isGetArms       = false;
        currentState    = State.Battle;

        //抓取對應屬性
        Player          = GameObject.Find("Player").gameObject;
        ChatController  = GameObject.Find("ChatController").GetComponent<ChatController>();
        NPC_Chat        = transform.Find("Chat").gameObject;
        target          = Enemy.transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Battle:
                if (isChase)
                {
                    isChase = false;
                    Sounds.White_TeachSay(0);                                   //白血球教學音效
                }
                Plot_Battle1();
                if (isBattleEnd)
                {
                    Plot_Battle2();
                }
                break;
            case State.TeachArm:
                if (ChatNum == 0)
                {
                    if (isChat)
                    {
                        //開啟對話框
                        NPC_Chat.SetActive(true);
                        //寒暄...
                        GetChat();
                        Sounds.White_TeachSay(1);                               //白血球教學音效
                        Invoke("ChatUp", 6.8f);                                 //6.8秒後進行下一段對話
                    }
                }
                if (ChatNum == 1)
                {
                    if (isChat)
                    {
                        //拿取抗生素槍
                        GetChat();
                        Sounds.White_TeachSay(2);                               //白血球教學音效
                        //生成武器
                        Antibiotic_Gun.SetActive(true);
                        Antibiotic_Gun.transform.parent = null;
                    }
                    if (isGetArms)
                    {
                        //獲得武器...
                        ChatUp();                                               //進行下一段對話
                    }
                }
                if (ChatNum == 2)
                {
                    if (isChat)
                    {
                        //切換模式型態教學
                        GetChat();
                        Sounds.White_TeachSay(3);                               //白血球教學音效
                    }
                    if (Player.transform.Find("[CameraRig]").transform.Find("Controller (right)").transform.GetComponent<MouseControl>().WeaponMode == 1)
                    {
                        //如果手上模式為武器
                        ChatUp();                                               //進行下一段對話
                    }
                }
                if (ChatNum == 3)
                {
                    if (isChat)
                    {
                        GetChat();
                        Sounds.White_TeachSay(4);                               //白血球教學音效
                    }
                    //介紹抗生素槍特性即如何使用教學
                    if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        this.GetComponent<Rigidbody>().isKinematic = true;      //把白血球固定住
                        Player.GetComponent<PlayerMovement>().enabled = true;   //玩家教學完成可移動
                        ChatUp();                                               //進行下一段對話
                    }
                }
                if (ChatNum == 4)
                {
                    if (isChat)
                    {
                        //訓練完成，預祝順利
                        GetChat();
                        Sounds.White_TeachSay(5);                               //白血球教學音效
                        Invoke("CloseChat", 7f);                                //7秒後關閉對話窗
                    }
                }
                break;
        }
        
    }
    void Plot_Battle1()
    {
        float distance = Vector3.Distance(pos[0].position, transform.position);     //白血球NPC與Pos0路徑的距離

        WhiteAni.SetFloat("Speed", 0.67f);                                          //奔跑動畫
        transform.DOMove(pos[0].position, _moveDuration).SetDelay(1f);              //延遲1秒後，白血球跑去Pos1路徑
        if (distance < 1f && !isBattleEnd)
        {
            //白血球與Pos0的距離小於1時並且要在戰鬥沒結束時...
            WhiteAni.SetFloat("Speed", 0f);                                         //轉為等待動畫
            WhiteAni.SetBool("Aiming", true);                                       //舉槍預備動畫
            Invoke("EnemyDead",2f);                                                 //2秒後怪物死掉
            StartCoroutine("Wait2Seconds");    
            WhiteAni.SetTrigger("Attack");                                          //攻擊動畫
        }
    }

    void Plot_Battle2()
    {
        float distance = Vector3.Distance(pos[1].position, transform.position);     //白血球NPC與Pos1路徑的距離
        transform.DOMove(pos[1].position, 3f);                                       //延遲2秒後，白血球跑去Pos1路徑
        if (distance < 1f)
        {
            //白血球與Pos1的距離小於1時...
            transform.DOPause();                                                    //停止DoMove動作
            WhiteAni.SetFloat("Speed", 0f);                                         //轉為等待動畫
            FaceTarget();                                                           //面向玩家
            Invoke("NextState", 2f);                                                //2秒後，使用手槍教學狀態
        }
    }

    public void ChatNext(RaycastHit hit)
    {
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            if (hit.transform.CompareTag("ChatButton")) //如果碰到螢幕
            {
                if (ChatNum < ChatController.TeachChat_White.Length)
                {
                    ChatUp();
                }
            }
            if (hit.transform.name == "Teach_WhiteBloodCell") //如果碰到螢幕
            {
                NPC_Chat.gameObject.SetActive(true);
                isChat = true;
            }
        }
    }

    public void TeachArms(RaycastHit hit)
    { 
        //如果是抗生素槍可進行...
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            //Debug.Log("IN"+hit.transform.name+"/"+ hit.transform.tag);
            if (hit.transform.name == "Antibiotic_Gun")
            {
           
                Destroy(hit.transform.gameObject, 1.5f);                              //1.5秒後消失
                //左鍵拿起時...
                hit.transform.position = theDest.position;
                hit.transform.parent = theDest;                                            //拿到手上
                isGetArms = true;
            }
        }
    }

    IEnumerator Wait2Seconds()
    {
        yield return new WaitForSeconds(2f);
        //怪物死亡後
        WhiteAni.SetBool("Aiming", false);                                          //停止舉槍預備動畫
        WhiteAni.SetFloat("Speed", 0.67f);                                          //奔跑動畫
        isBattleEnd = true;
    }

    void EnemyDead()
    {
        Destroy(Enemy);
    }

    void FaceTarget()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void NextState()
    {
        currentState = State.TeachArm;  //教學模式
    }

    void GetChat()
    {
        isChat = false;
        NPC_Chat.GetComponent<SpriteRenderer>().sprite = ChatController.TeachChat_White[ChatNum];
    }

    public void ChatUp()
    {
        isChat = true;
        if (ChatNum < 5)    //確保超出
        {
            ChatNum++;
        }
    }

    void CloseChat()
    {
        NPC_Chat.gameObject.SetActive(false);
        TriggerController.MonsterCard[0].SetActive(true);
        Invoke("CloseCard", 10f);   //十秒後自動關閉怪獸卡片
    }

    void CloseCard()
    {
        if (TriggerController.MonsterCard[0].active == true)
        {
            TriggerController.MonsterCard[0].SetActive(false);
        }
    }
}
