using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Teach_Glucose : MonoBehaviour
{
    //紅血球狀態(等待、教學葡萄糖)
    public enum State
    {
        Idle,
        TeachGlucose,
    }
    public State currentState;                      //目前白血球狀態

    [SerializeField] private int ChatNum = 0;       //聊天圖片編號
    private ChatController  ChatController;         //聊天腳本
    private GameObject      NPC_Chat;               //紅血球聊天物件

    private Sounds          Sounds;                 //音效控制
    private Animator        RedAni;                 //紅血球動畫控制

    public  Transform       pos;                    //紅血球走路軌跡位置
    [Range(1.0f, 20.0f), SerializeField]
    private float           _moveDuration = 6.0f;   //DoMove時間
    private GameObject      Player;                 //玩家物件
    public  ArrowMove       ArrowMove;             //箭頭Code
    
    public static bool            isLookInf   = true;     //判斷是否有查詢資訊
    [SerializeField] private bool            isChat      = true;     //聊天
    [SerializeField] private bool            isChatTime  = false;    //判斷視窗冷卻度是否關閉

    void Awake()
    {
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        RedAni = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //初始化
        currentState    = State.Idle;
        isChat          = true;
        isLookInf       = false;
        isChatTime      = false;

        //抓取對應屬性
        Player          = GameObject.Find("Player").gameObject;
        ChatController  = GameObject.Find("ChatController").GetComponent<ChatController>();
        NPC_Chat        = transform.Find("Chat").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (ChatNum == 0)
                {
                    currentState = State.TeachGlucose;
                }
                if (ChatNum == 4)
                {
                    if (isChat && !isChatTime)
                    {
                        isChatTime = true;
                        //到販賣機或取葡萄糖
                        GetChat();
                        Sounds.Glucose_TeachSay(4);     //紅血球能量飲教學音效
                        Invoke("CloseChat", 4.5f);
                    }
                }
                break;
            case State.TeachGlucose:
                if (ChatNum == 0)
                {
                    if (isChat)
                    {
                        //詢問說出葡萄糖
                        GetChat();
                        Sounds.Glucose_TeachSay(0);     //紅血球能量飲教學音效
                        Invoke("ChatUp", 6.5f);         //5.5秒後下一個
                    }
                }
                if (ChatNum == 1)
                {
                    if (isChat)
                    {
                        //檢查血量與能量
                        GetChat();
                        Sounds.Glucose_TeachSay(1);     //紅血球能量飲教學音效
                        Invoke("ChatUp", 5.5f);         //5.5秒後下一個
                    }
                }
                if (ChatNum == 2)
                {
                    if (isLookInf)
                    {
                        if (isChat)
                        {
                            //詢問說出葡萄糖
                            GetChat();
                            Sounds.Glucose_TeachSay(2);     //紅血球能量飲教學音效
                            Invoke("ChatUp", 5.5f);     //5.5秒後下一個
                        }
                    }
                }
                if (ChatNum == 3)
                {
                    if (isChat)
                    {
                        //提醒務必去領
                        GetChat();
                        Sounds.Glucose_TeachSay(3);     //紅血球能量飲教學音效
                        Invoke("ChatUp", 8.5f);         //8.5秒後下一個
                    }
                }
                if (ChatNum == 4)
                {
                    if (isChat)
                    {
                        //到販賣機或取葡萄糖
                        GetChat();
                        Sounds.Glucose_TeachSay(4);     //紅血球能量飲教學音效
                        StartCoroutine("GoPos");

                        //開啟箭頭
                        ArrowMove.arrows[3].SetActive(true);
                        
                        //腳色可移動
                        Player.GetComponent<PlayerMovement>().enabled=true;

                        currentState = State.Idle;
                    }
                }
                break;
        }
    }

    public void ChatNext(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hit.transform.CompareTag("ChatButton")) //如果碰到螢幕
            {
                if (ChatNum < ChatController.TeachChat_Glucose.Length)
                {
                    ChatUp();
                }
            }
            if (hit.transform.name == "Teach_Glucose") //如果碰到螢幕
            {
                NPC_Chat.gameObject.SetActive(true);
                isChat = true;
            }
        }
    }

    IEnumerator GoPos()
    {
        ArrowMove.enabled = true;
        yield return new WaitForSeconds(3f);
        CloseChat();
        transform.DORotate(new Vector3(0,-10,0),1f);
        RedAni.SetFloat("Speed",0.4f);
        yield return new WaitForSeconds(1f);
        transform.DOMove(pos.position,_moveDuration);
        yield return new WaitForSeconds(_moveDuration);
        transform.DORotate(new Vector3(0,-180,0),2.5f);
        RedAni.SetFloat("Speed",0f);
    }
    void FaceTarget()
    {
        Debug.Log(0);
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void GetChat()
    {
        isChat = false;
        NPC_Chat.GetComponent<SpriteRenderer>().sprite = ChatController.TeachChat_Glucose[ChatNum];
    }

    public void ChatUp()
    {
        isChat = true;
        if (ChatNum < 4)    //確保超出
        {
            ChatNum++;
        }
    }

    void CloseChat()
    {
        isChatTime = false;
        NPC_Chat.gameObject.SetActive(false);
    }
}
