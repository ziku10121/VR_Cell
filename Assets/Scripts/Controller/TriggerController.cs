using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Valve.VR;

public class TriggerController : MonoBehaviour
{
    public CameraRay CameraInfo;
    public GameObject[] Body_parts;      //整個人體場景部位
    public GameObject[] NPC;                //整個NPC
    public GameObject[] Enemy;            //整個怪物
    public GameObject[] Obstacle;         //整個障礙物
    public GameObject[] HeartThing;     //心臟裡面物件
    public GameObject NextPos;            //第一關到第二關初始位子
    public GameObject SendVFX;          //傳送特效
    public GameObject Box;                 //運送O2/CO2箱子
    public GameObject Video;              //教學布幕
    public GameObject StandObj;         //電梯的固定位子
    private Sounds Sounds;
    public GameObject[]   GateInformation;  //夾道門顯示禁止通行
    public GameObject Warning;                  //肺部警告招牌
    public GameObject[] MonsterCard;         //怪物卡片資訊

    private float           SendTime;                       //傳送冷卻時間
    private ArrowMove ArrowMove;                       //箭頭移動
    private PlayerMovement PlayerMovement;        //玩家移動Code
    private QuestionController QuestionController;  //玩家移動Code
    private LungChange LungChange;                    //判斷二氧化碳箱子是否放到換氧位置

    public bool[] Battle=new bool[5];

    void Awake()
    {
        ArrowMove = GameObject.Find("Arrow_Teach").GetComponent<ArrowMove>();
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        LungChange = GameObject.Find("Box_CO2").GetComponent<LungChange>();
    }
    void Start()
    {
        SendTime = 3f;
        //NPC[2].SetActive(false);
        //Enemy[1].SetActive(false);
        LungChange = GameObject.Find("Box_CO2").GetComponent<LungChange>();
        PlayerMovement = GetComponent<PlayerMovement>();
        QuestionController = GameObject.Find("QuestionController").GetComponent<QuestionController>();

        for (int i = 0; i < Battle.Length; i++)
        {
            Battle[i] = false;
        }
    }
    
    void FixedUpdate()
    {
        if (Battle[0])
        {
            //MRSA戰鬥
            if (Enemy[1] == null && Enemy[2] == null && Enemy[3] == null && Enemy[4] == null && Enemy[5] == null)
            {
                MonsterCard[1].SetActive(true); //開啟怪物卡片
                Invoke("CloseCard", 10f);       //十秒後自動關閉怪獸卡片

                Battle[0] = false;
                Sounds._BGM(Sounds.BGM[1]);     //播放心臟音效
            }
        }
        if (Battle[1])
        {
            //肺炎鏈球菌
            if (Enemy[6] == null && Enemy[7] == null && Enemy[8] == null && Enemy[9] == null)
            {
                MonsterCard[2].SetActive(true); //開啟怪物卡片
                Invoke("CloseCard", 10f);       //十秒後自動關閉怪獸卡片

                //箭頭程式打開(肺部箭頭)
                ArrowMove.enabled = true;
                ArrowMove.arrows[4].SetActive(true);
                ArrowMove.arrows[5].SetActive(true);
                ArrowMove.arrows[6].SetActive(true);

                Battle[1] = false;
                Sounds._BGM(Sounds.BGM[1]);     //播放心臟音效
            }
        }
        if (Battle[2])
        {
            //仙人掌桿菌
            if (Enemy[10] == null && Enemy[11] == null && Enemy[12])
            {
                MonsterCard[3].SetActive(true); //開啟怪物卡片
                Invoke("CloseCard", 10f);       //十秒後自動關閉怪獸卡片

                Battle[2] = false;
                Sounds._BGM(Sounds.BGM[1]);     //播放心臟音效
            }
        }
        if (Battle[3])
        {
            //腸炎弧菌
            if (Enemy[13] == null && Enemy[14] == null && Enemy[15])
            {
                MonsterCard[4].SetActive(true); //開啟怪物卡片
                Invoke("CloseCard", 10f);       //十秒後自動關閉怪獸卡片

                //開啟箭頭
                ArrowMove.enabled = true;
                ArrowMove.arrows[8].SetActive(true);

                Battle[3] = false;
                Sounds._BGM(Sounds.BGM[0]);     //播放Play音效
            }
        }
        if (Battle[4])
        {
            //海獸胃腺蟲
            if (Enemy[16] == null)
            {
                MonsterCard[5].SetActive(true); //開啟怪物卡片
                Invoke("CloseCard", 10f);       //十秒後自動關閉怪獸卡片

                Battle[4] = false;
                Sounds._BGM(Sounds.BGM[0]);     //播放Play音效
            }
        }

    }

    void OnTriggerEnter(Collider e)
    {

        if (e.name == "NPCTrigger")
        {
            //Scence場景控制 - 開關Level1、Heart、Aorta、Road1、Road3、Road_End
            Body_parts[0].SetActive(false);
            Body_parts[2].SetActive(false);
            Body_parts[3].SetActive(false);
            Body_parts[4].SetActive(false);
            Body_parts[6].SetActive(false);
            Body_parts[7].SetActive(false);

            //NPC，紅血球開啟Chat(ComeHere)
            NPC[0].GetComponent<Tech_RedBloodCell>().enabled = true;
            NPC[0].transform.Find("Chat").gameObject.SetActive(true);

            //怪物開啟
            Enemy[0].SetActive(true);

            Destroy(e.gameObject);
        }

        if (e.name == "TeachCellWork")
        {
            //NPC，紅血球Chat(SayHi)
            NPC[0].GetComponent<Tech_RedBloodCell>().ChatUp();

            Destroy(e.gameObject);
            
        }

        if (e.name == "NPC_ForArms")
        {
            //Scence場景控制 - 開啟Heart
            Body_parts[2].SetActive(true);
            HeartThing[0].SetActive(false);
            HeartThing[1].SetActive(false);
            HeartThing[2].SetActive(false);
            HeartThing[3].SetActive(false);
            HeartThing[4].SetActive(false);
            HeartThing[5].SetActive(false);
            //Scence場景控制 - NPC3~7開啟
            NPC[3].SetActive(true);
            NPC[4].SetActive(true);
            NPC[5].SetActive(true);
            NPC[6].SetActive(true);
            NPC[7].SetActive(true);
            //NPC，白血球開啟動畫
            NPC[1].SetActive(true);

            //怪物追逐
            Enemy[0].GetComponent<Teach_EnemyController>().isPlotMode = true;

            //停止移動
            PlayerMovement.enabled = false;

            Destroy(e.gameObject);
        }

        if (e.name == "ObstacleTrigger")
        {
            //判斷是否持有箱子
            //GameObject child1;
            //GameObject child2;
            if (Box.transform.parent.name == "Destination" || Box.transform.parent.name == "BoxTrigger")
            {
                //箱子在手上或推車上...
                //需要拿著箱子
                Destroy(Obstacle[0]);
                Destroy(e.gameObject);
            }
        }

        if (e.name == "GlucoseTeach")
        {
            //CameraInfo開啟
            CameraInfo.enabled = true;
            //NPC，紅血球開啟Chat(葡萄糖)
            NPC[2].GetComponent<Teach_Glucose>().enabled = true;
            NPC[2].transform.Find("Chat").gameObject.SetActive(true);

            //停止移動
            PlayerMovement.enabled = false;

            Destroy(e.gameObject);
        }

        if (e.name == "GateTrigger")
        {
            //開啟問題1
            QuestionController.B_Select[0].SetActive(true);
            QuestionController.GetGate(0);

            Destroy(e.gameObject);
        }

        if (e.name == "VideoTrigger")
        {
            //停止移動
            PlayerMovement.enabled = false;

            //開啟教學影片
            Video.GetComponent<VideoPlayer>().Play();
            Invoke("PlayerMove",96);     //96秒後看完影片可移動

            Destroy(e.gameObject);
        }

        if (e.name == "Enter_Heart")
        {
            //Scence場景控制，寺廟(左心)
            HeartThing[0].SetActive(true);
            HeartThing[1].SetActive(true);
            HeartThing[2].SetActive(true);
            HeartThing[3].SetActive(false);
            HeartThing[4].SetActive(false);
            HeartThing[5].SetActive(false);

            Destroy(e.gameObject);
        }

        if (e.name == "HeartMusic")
        {
            //心臟背景音效
            Sounds._BGM(Sounds.BGM[1]);

            Destroy(e.gameObject);
        }

        if (e.name == "HRA_Trigger")
        {
            Sounds._CloseDoor(5);               //右心房關門音效
            //Scence場景控制，大靜脈、NPC3~7開啟
            Body_parts[1].SetActive(false);
            NPC[3].SetActive(false);
            NPC[4].SetActive(false);
            NPC[5].SetActive(false);
            NPC[6].SetActive(false);
            NPC[7].SetActive(false);
            //右心房關門
            Obstacle[1].GetComponent<Animator>().SetBool("close",true);

            //回答問題2、3、4
            QuestionController.B_Select[1].SetActive(true);
            QuestionController.GetGate(1);

            Destroy(e.gameObject);
        }

        if (e.name == "HRV_Trigger")
        {

            
            Sounds._Church_Bell();      //右心室敲鐘音效
            Invoke("HRVOpenDoor",9f);   //右心室9秒開門
            

            //開啟戰鬥一怪物待命

            Destroy(e.gameObject);
        }

        if (e.name == "EnemyBattle1")
        {
            

            //怪物開始戰鬥1
            for (int i = 1; i <= 5; i++)
            {
                Enemy[i].GetComponent<EnemyController>().isBattle = true;
            }

            //開啟戰鬥二怪物待命
            Destroy(e.gameObject);
        }

        if (e.name == "EnterLung")
        {
            Sounds._BGM(Sounds.BGM[2]);                                 //戰鬥背景音效
            Battle[1] = true;
            Sounds._AutoDoor(1);                                        //肺部門打開音效(自動門)
            Obstacle[4].GetComponent<Animator>().SetBool("open", true); //肺部門打開(進入)

            //肺炎鏈球菌出現(開啟6~9)
            for (int i = 6; i <= 9; i++)
            {
                Enemy[i].SetActive(true);
            }

            Destroy(e.gameObject);
        }

        if (e.name == "EnemyBattle2")
        {
            Sounds._AutoDoor(1);                                        //肺部門關閉音效(自動門)
            Obstacle[4].GetComponent<Animator>().SetBool("open", false);//肺部門關閉(進入)

            //怪物開始戰鬥2
            for (int i = 6; i <= 9; i++)
            {
                Debug.Log("SP"+i);
                Enemy[i].GetComponent<EnemyController>().isBattle = true;
            }

            Destroy(e.gameObject);
        }

        if (e.name == "LungTrigger")
        {
            if (LungChange.isLung)
            {
                ArrowMove.arrows[6].SetActive(false);       //關閉箭頭
                ArrowMove.enabled = false;                  //關閉箭頭程式
                Invoke("CloseWarning", 0f);                 //迅速關閉警告

                //回答問題5
                QuestionController.B_Select[4].SetActive(true);
                QuestionController.B_Select[4].transform.GetChild(0).gameObject.SetActive(true);
                //開啟題目選項
                Invoke("OpenBQ5", 1f);
                QuestionController.GetGate(4);

                Destroy(e.gameObject);
            }
            else
            {
                Warning.SetActive(true);        //警告開啟
                Invoke("CloseWarning", 3f);     //兩秒後關閉警告
            }
        }

        if (e.name == "ExitLung")
        {
            Sounds._AutoDoor(1);                                       //肺部門打開音效(自動門)
            Obstacle[5].GetComponent<Animator>().SetBool("open", true);//肺部門打開(離開)

            //仙人掌桿菌出現(開啟10~12)
            for (int i = 10; i <= 12; i++)
            {
                Enemy[i].SetActive(true);
            }
            

            Destroy(e.gameObject);
        }

        if (e.name == "EnemyBattle3")
        {
            Sounds._BGM(Sounds.BGM[2]);                                 //戰鬥背景音效
            Battle[2] = true;

            Sounds._AutoDoor(1);                                        //肺部門關閉音效(自動門)
            Obstacle[5].GetComponent<Animator>().SetBool("open", false);//肺部門打開(離開)

            //Scence場景控制，寺廟(右心)
            HeartThing[0].SetActive(false);
            HeartThing[1].SetActive(false);
            HeartThing[2].SetActive(false);
            HeartThing[3].SetActive(true);
            HeartThing[4].SetActive(true);
            HeartThing[5].SetActive(true);

            //怪物開始戰鬥3
            for (int i = 10; i <= 12; i++)
            {
                Enemy[i].GetComponent<EnemyController>().isBattle = true;
            }

            Destroy(e.gameObject);
        }

        if (e.name == "HLA_Trigger")
        {

            //回答問題6、7
            QuestionController.B_Select[5].SetActive(true);
            QuestionController.GetGate(5);

            Destroy(e.gameObject);
        }

        if (e.name == "HLV_Trigger")
        {
            //Scence場景控制，主動脈，Road1~RoadEnd
            Body_parts[3].SetActive(true);
            Body_parts[4].SetActive(true);
            Body_parts[5].SetActive(true);
            Body_parts[6].SetActive(true);
            Body_parts[7].SetActive(true);
            
            Sounds._TrapDoor(11);                                       //左心室門開啟音效
            Obstacle[3].GetComponent<Animator>().SetBool("open", true); //左心室門開起來

            Destroy(e.gameObject);
        }

        if (e.name == "GoAorta")
        {
            //開啟箭頭
            ArrowMove.enabled = true;
            ArrowMove.arrows[7].SetActive(true);

            //Play背景音效
            Sounds._BGM(Sounds.BGM[0]);

            //腸炎弧菌出現(開啟13~15)
            for (int i = 13; i <= 15; i++)
            {
                Enemy[i].SetActive(true);
            }

            Destroy(e.gameObject);
        }

        if (e.name == "GoRoad")
        {
            //關閉箭頭
            ArrowMove.arrows[7].SetActive(false);
            ArrowMove.enabled = false;
        }

        if (e.name == "EnemyBattle4")
        {
            //戰鬥背景音效
            Sounds._BGM(Sounds.BGM[2]);
            Battle[3] = true;

            //Scence場景控制，心臟關閉(右心房)
            HeartThing[3].SetActive(false);
            HeartThing[4].SetActive(false);
            HeartThing[5].SetActive(false);

            //怪物開始戰鬥4
            for (int i = 13; i <= 15; i++)
            {
                Enemy[i].GetComponent<EnemyController>().isBattle = true;
            }

            Destroy(e.gameObject);
        }

        if (e.name == "EnterElevator")
        {
            //關閉箭頭
            ArrowMove.arrows[8].SetActive(false);
            ArrowMove.enabled = false;
            

            Sounds._AutoDoor(12);                                                                   //自動門音效
            StandObj.transform.parent.gameObject.GetComponent<Animator>().SetBool("open", true);    //搭電梯打開
        }

        if (e.name == "Elevator")
        {
            //海獸胃腺蟲出現(16)
            Enemy[16].SetActive(true);

            //搭電梯
            transform.position = StandObj.transform.position;
            this.transform.parent = StandObj.transform;
            this.GetComponent<PlayerMovement>().enabled = false;
        }

        if (e.name == "EnemyBattle5")
        {
            //戰鬥背景音效
            Sounds._BGM(Sounds.BGM[2]);
            Battle[4] = true;

            Sounds._AutoDoor(12);                                                                   //搭電梯關門音效(自動門)
            StandObj.transform.parent.gameObject.GetComponent<Animator>().SetBool("open", false);   //搭電梯關門
            Invoke("DownElevator", 1.5f);

            //怪物開始戰鬥5
            Enemy[16].GetComponent<EnemyController>().isBattle = true;

            Destroy(e.gameObject);
        }
    }

    private void OnTriggerStay(Collider e)
    {
        if (e.name == "ProtalTrigger")
        {
            //待在傳送空間...開啟傳送特效、關閉指令ICON、施展傳送空間
            SendVFX.SetActive(true);
            e.transform.GetChild(0).gameObject.SetActive(false);
            SendTime -= Time.deltaTime;
            SteamVR_Fade.Start(Color.black, 3f,true);
            if (SendTime <= 1f)
            {
                Sounds._Shuttle();
            }
            if (SendTime <= 0)
            {
                //移到第二關指定地點並把傳送門關閉
                transform.position = NextPos.transform.position;
                SteamVR_Fade.Start(Color.clear, 3f,true);
                e.gameObject.SetActive(false);
            }
        }
    }
    void OnTriggerExit(Collider e)
    {
        if (e.name == "ProtalTrigger")
        {
            //離開傳送門...關閉特效、開啟指定ICON、傳送空間冷卻初始化
            SendVFX.SetActive(false);
            e.transform.GetChild(0).gameObject.SetActive(true);
            SteamVR_Fade.Start(Color.clear, 0.1f, true);
            SendTime = 3f;
            Sounds.audio[1].Stop();
        }
        if (e.name == "GateTriggerClose")
        {
            //匣道門關閉
            QuestionController.Gate.GetComponent<Animator>().SetBool("close", true);
            QuestionController.Gate.GetComponent<Animator>().SetBool("open", false);

            //匣道門資訊
            GateInformation[0].SetActive(false);
            GateInformation[1].SetActive(false);
            GateInformation[2].SetActive(false);
            GateInformation[3].SetActive(true);
            GateInformation[4].SetActive(true);
            GateInformation[5].SetActive(true);

            Destroy(e.gameObject);
        }
        if (e.name == "HRVClose_Trigger")
        {
            Sounds._BGM(Sounds.BGM[2]);                                 //戰鬥背景音效
            Battle[0] = true;

            Sounds._CloseTrapDoor(8);                                   //右室門關起音效
            Obstacle[2].GetComponent<Animator>().SetBool("open", false);//右室門關起來

            //血小板出現
            NPC[8].SetActive(true);

            //金黃色葡萄球菌出現(開啟1~5)
            for (int i = 1; i < 6; i++)
            {
                Enemy[i].SetActive(true);
            }

            Destroy(e.gameObject);
        }
        if (e.name == "HLVClose_Trigger")
        {
            
            Sounds._CloseTrapDoor(11);                                  //左心室關門音效
            Obstacle[3].GetComponent<Animator>().SetBool("open", false);//左心室關起來

            Destroy(e.gameObject);
        }

        if (e.name == "EnterElevator")
        {
            Sounds._AutoDoor(12);           //自動門音效
            Invoke("CloseElevator", 2f);    //搭電梯關門
        }
    }
    
    void PlayerMove()
    {
        PlayerMovement.enabled = true;
    }

    void HRVOpenDoor()
    {
        Sounds._TrapDoor(8);                                            //右心室門開啟音效
        Obstacle[2].GetComponent<Animator>().SetBool("open", true);     //右心室門開起
    }

    void CloseWarning()
    {
        Warning.SetActive(false);
    }

    void OpenBQ5()
    {
        QuestionController.B_Select[4].transform.GetChild(1).gameObject.SetActive(true);
        QuestionController.B_Select[4].transform.GetChild(2).gameObject.SetActive(true);
        QuestionController.B_Select[4].transform.GetChild(3).gameObject.SetActive(true);
        QuestionController.B_Select[4].transform.GetChild(4).gameObject.SetActive(true);
        QuestionController.B_Select[4].transform.GetChild(5).gameObject.SetActive(true);
    }

    void CloseElevator()
    {
        StandObj.transform.parent.gameObject.GetComponent<Animator>().SetBool("up", false);
    }

    void CloseCard()
    {
        if (MonsterCard[1].active == true)
        {
            MonsterCard[1].SetActive(false);
        }
        if (MonsterCard[2].active == true)
        {
            MonsterCard[2].SetActive(false);
        }
        if (MonsterCard[3].active == true)
        {
            MonsterCard[3].SetActive(false);
        }
        if (MonsterCard[4].active == true)
        {
            MonsterCard[4].SetActive(false);
        }
        if (MonsterCard[5].active == true)
        {
            MonsterCard[5].SetActive(false);
        }
    }
}
