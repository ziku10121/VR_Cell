using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chat_Knowledge : MonoBehaviour
{
    public int              ChatNum =0;                 //聊天圖片編號
    public float            Closetime = 0;              //開啟後幾秒關閉視窗
    private ChatController  ChatController;             //聊天腳本
    private GameObject      NPC_Chat;                   //紅血球聊天物件
    private Quaternion      OriginAngels;               //NPC本身旋轉角度
    private GameObject      Player;                     //玩家物件

    private AudioSource     KnowledgeSource;              //知識音效
    private Animator        RedAni;                     //動畫控制
    public string           NameAni;                    //播放啟動

    public bool            isChat          = true;     //判斷是否開啟對話窗

    // Start is called before the first frame update
    void Start()
    {
        //初始化
        isChat          = true;

        //抓取對應屬性
        OriginAngels = this.transform.rotation;
        Player = GameObject.Find("Player").gameObject;
        ChatController  = GameObject.Find("ChatController").GetComponent<ChatController>();
        NPC_Chat = transform.Find("Chat").gameObject;
        KnowledgeSource = GetComponent<AudioSource>();
        RedAni = GetComponent<Animator>();
        RedAni.SetBool(NameAni, true);

        Debug.Log("Rotation：" + OriginAngels.x+"/"+ OriginAngels.y + "/" + OriginAngels.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!isChat)
        {
            FaceTarget();
        }
        if (isChat)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, OriginAngels, 5f * Time.deltaTime);
        }
    }
    void GetChat(string Cells)
    {
        NPC_Chat.SetActive(true);
        if (Cells == "Red")
        {
            NPC_Chat.GetComponent<SpriteRenderer>().sprite = ChatController.KnowChat_Red[ChatNum];
        }
        if (Cells == "White")
        {
            NPC_Chat.GetComponent<SpriteRenderer>().sprite = ChatController.KnowChat_White[ChatNum];
        }
        if (Cells == "Platelet")
        {
            NPC_Chat.GetComponent<SpriteRenderer>().sprite = ChatController.KnowChat_Platelet[ChatNum];
            RedAni.SetBool(NameAni, false);
        }
    }

    public void Chat_Know(RaycastHit hit)
    {
        if (isChat)
        {
            if (hit.transform.CompareTag("NPC"))
            {
                isChat = false;
                //教學完(回話模式)
                KnowledgeSource.Play();
                GetChat(hit.transform.name);
                Invoke("CloseChat", Closetime);                 //8秒後關閉對話窗
            }
        }
    }

    void CloseChat()
    {
        isChat = true;
        NPC_Chat.gameObject.SetActive(false);
    }
    
    

    void FaceTarget()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
