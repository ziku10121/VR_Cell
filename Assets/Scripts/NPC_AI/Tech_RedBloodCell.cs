using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Tech_RedBloodCell : MonoBehaviour
{
    //教學紅血球狀態(等待、叫你過來、打招呼、教使用者把CO2放到車子、給地圖&任務)
    public enum State
    {
        Idle,
        Comehere,
        SayHi,
        TeachBox,
        ForMap
    }
    public  State           currentState;               //目前紅血球狀態

    private int             ChatNum=0;                  //聊天圖片編號
    private ChatController  ChatController;             //聊天腳本
    private GameObject      NPC_Chat;                   //紅血球聊天物件
    private Sounds          Sounds;                     //音效控制
    private Animator        RedAni;                     //紅血球動畫控制
    
    public  ArrowMove       ArrowMove;                  //箭頭Code
    public  Transform       theDest;                        //抓物件位置
    private GameObject      Pushcart_top;             //車子上面物件

    public  GameObject      Map;                        //血液循環圖物件
    public  GameObject      Task;                       //任務紙張物件


    [SerializeField]private bool            isPick          = true;     //判斷是否可進行
    public  bool            isTeachBox_Ok   = false;    //判斷是否完成教學動作    
    public  bool            isGetMap        = true;     //判斷是否拿起血液循環圖
    public  bool            isGetTask       = true;     //判斷是否拿起任務紙張
    private bool            isChat          = true;     //判斷是否開啟對話窗
    private bool            isChatTime      = false;    //判斷視窗冷卻度是否關閉

    void Awake()
    {
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始化
        isPick          = false;
        isTeachBox_Ok   = false;
        isGetMap        = false;
        isGetTask       = false;
        isChat          = true;
        isChatTime      = false;
        currentState    = State.Idle;

        //抓取對應屬性
        ChatController  = GameObject.Find("ChatController").GetComponent<ChatController>();
        RedAni          = GetComponent<Animator>();
        NPC_Chat        = transform.Find("Chat").gameObject;
        Pushcart_top    = GameObject.Find("Pushcart").transform.Find("BoxTrigger").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Idle:
                if(ChatNum==0)
                {
                    //一開始看到紅血球(叫我過去)狀態
                    
                    currentState =State.Comehere;
                }
                if (ChatNum == 4)
                {
                    if (isChat && !isChatTime)
                    {
                        isChatTime = true;
                        //教學完(回話模式)
                        GetChat();
                        Sounds.Red_TeachSay(4);         //紅血球音效
                        Invoke("CloseChat", 8f);        //8秒後關閉對話窗
                    }
                }
                break;
            case State.Comehere:

                if (ChatNum == 0)
                {
                    if (isChat)
                    {
                        GetChat();
                        Sounds.Red_TeachSay(0);         //紅血球音效
                    }
                }

                if (ChatNum == 1)
                {
                    //過來後打招呼準備
                    //Triiger踩到正確地方後進行下一段對話
                    RedAni.SetBool("SayHi", true);
                    currentState = State.SayHi;
                }
                break;
            case State.SayHi:
                
                if (ChatNum==1)
                {
                    if (isChat)
                    {
                        GetChat();
                        Sounds.Red_TeachSay(1);         //紅血球音效
                        //打招呼結束
                        Invoke("ChatUp", 7.5f);         //5秒後進行下一段對話
                    }
                }
                if (ChatNum == 2)
                {
                    //打完招呼完啟動教學準備
                    RedAni.SetBool("SayHi", false);
                    //開啟箭頭
                    ArrowMove.enabled=true;
                    ArrowMove.arrows[1].SetActive(true);
                    ArrowMove.arrows[2].SetActive(true);
                    currentState = State.TeachBox;
                }
                break;
            case State.TeachBox:
                isPick = true;                          //開啟檢箱子&推車

                if (isChat)
                {
                    GetChat();
                    Sounds.Red_TeachSay(2);             //紅血球音效
                }

                if(isTeachBox_Ok)
                {
                    //教學成功後啟動給地圖&任務準備
                    Invoke("ChatUp", 1.5f);             //1.5秒後進行下一段對話
                    Invoke("ForMap", 3f);               //3秒後給出地圖
                    currentState = State.ForMap;
                }
                break;
            case State.ForMap:
                if (isChat)
                {
                    GetChat();
                    Sounds.Red_TeachSay(3);              //紅血球音效
                }
                if (isGetMap && isGetTask)
                {
                    //血液循環圖與任務紙張都拿取後啟動NPC(回話模式)
                    Invoke("ChatUp", 2f);               //2秒後進行下一段對話
                    currentState =State.Idle;
                }
                break;
        }
    }
    
    void GetChat()
    {
        isChat = false;
        NPC_Chat.GetComponent<SpriteRenderer>().sprite=ChatController.TeachChat_Red[ChatNum];
    }

    void CloseChat()
    {
        isChatTime = false;
        NPC_Chat.gameObject.SetActive(false);
    }

    void SayHi()
    {
        NPC_Chat.GetComponent<SpriteRenderer>().sprite=ChatController.TeachChat_Red[ChatNum];
    }
    
    public void ChatNext(RaycastHit hit)
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (hit.transform.CompareTag("ChatButton")) //如果碰到螢幕
            {
                if(ChatNum<ChatController.TeachChat_Red.Length)
                {
                    ChatUp();
                }
            }
            if (hit.transform.name == "Teach_RedBloodCell") //如果碰到紅血球
            {
                NPC_Chat.gameObject.SetActive(true);
                isChat = true;
            }
        }
    }
    
    public void PickUp(RaycastHit hit)
    {
        //可使用箱子&推車
        if (isPick)
        {
            if (hit.transform.CompareTag("Box"))
            {
                //如果是箱子就可進行...
                if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    //關閉箭頭
                    ArrowMove.arrows[1].SetActive(false);
                    
                    hit.transform.gameObject.GetComponent<Collider>().isTrigger = true;
                    hit.transform.gameObject.GetComponent<Rigidbody>().useGravity = false;  //關閉重力
                    hit.transform.position = theDest.position;
                    hit.transform.parent = theDest;                                         //拿到手上
                }
                if (SteamVR_Actions._default.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand))
                {
                    //左鍵放下時...
                    hit.transform.gameObject.GetComponent<Collider>().isTrigger=false;
                    hit.transform.parent = null;
                    hit.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;  //關閉重力
                }
            }
            
            if (hit.transform.name == "Pushcart")
            {
                //如果是推車就可進行...
                if (Pushcart_top.GetComponent<BoxController>().isObject)
                {
                    //推車上面有CO2或O2箱子可進行...
                    if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        //左鍵拿起時...

                        //關閉箭頭
                        ArrowMove.enabled=false;
                        ArrowMove.arrows[2].SetActive(false);
                        
                        hit.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;  //開啟固定不影響重力
                        hit.transform.position = theDest.transform.position;
                        hit.transform.parent = theDest;                                     //拿到手上
                        hit.transform.localEulerAngles = new Vector3(0, 0, 0);
                        isTeachBox_Ok = true;                                                  //推車教學完成
                    }
                    if (SteamVR_Actions._default.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand))
                    {
                        //左鍵放下時...
                        hit.transform.parent = null;
                        hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;  //關閉固定不影響重力
                    }
                }
            }
            if (hit.transform.name == "Map" || hit.transform.name =="Task")
            {
                //如果是血液循環圖或者任務紙張可進行...
                if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    if (hit.transform.name=="Map")
                    {
                        isGetMap = true;
                    }
                    if (hit.transform.name == "Task")
                    {
                        isGetTask = true;
                    }
                    //左鍵拿起時...
                    Destroy(hit.transform.gameObject,2f);                                   //2秒後消失
                    hit.transform.position = theDest.position;
                    hit.transform.parent = theDest;                                            //拿到手上
                    hit.transform.localEulerAngles = new Vector3(0, 0, 0);
                    hit.transform.localScale = new Vector3(1f, 1f, 1f);
                                                      
                    
                }
            }
        }
    }

    public void ChatUp()
    {
        isChat = true;
        if (ChatNum < 5)    //確保超出
        {
            ChatNum++;
        }
    }

    void ForMap()
    {
        //生成ForMap & ForTask)
        Map.SetActive(true);
        Task.SetActive(true);
    }
}
