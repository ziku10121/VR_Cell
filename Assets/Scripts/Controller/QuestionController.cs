using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class QuestionController : MonoBehaviour
{
    public Material[]   Question;
    public GameObject[] Select;
    public Sprite[]     JudgeImage;
    private int[]       Question_Anser=new int[9]{3,1,1,2,2,1,3,4,2};

    public GameObject[] B_Select;
    public Sprite[]     B_JudgeImage;
    private int[]       B_Question_Anser=new int[7]{4,2,4,4,4,2,4};

    public static int BottleNum = 0;
    [SerializeField]private int NowAnser    =0; //現在的答案
    [SerializeField]private int SelectAnser =0; //現在選擇的答案
    [SerializeField]private Sprite[]   NowJudgeImage=new Sprite[4];
    [SerializeField]private GameObject NowSelect   =null; //現在選擇的販賣機
    [SerializeField]private GameObject NowMaterial =null; //現在選擇販賣機的Material
    public Material     GetCansScreen;             //葡萄糖已拿取Material
    private Sounds Sounds;
    [SerializeField]private int HRA_num;

    [SerializeField]private bool isChoice   =true;      //判斷現在選擇的答案
    [SerializeField]private bool isChoice_Part1 = true; //判斷匣道門選擇的答案
    [SerializeField]private bool isChoice_Part2 = true; //判斷右心房選擇的答案
    [SerializeField]private bool isChoice_Part3 = true; //判斷肺部選擇的答案
    [SerializeField]private bool isChoice_Part4 = true; //判斷左心室選擇的答案
    //private bool isPreQ_Ok = true;                    //判斷上題目是否答對
    
    public  GameObject   Gate;                             //匣道門物件
    public  GameObject   HRAGate;                          //右心房門物件
    public  GameObject   HLAGate;                          //右心房門物件
    private Animator     GateAni;                          //匣道門動畫控制器
    private Animator     HRAGateAni;                       //匣道門動畫控制器
    private Animator     HLAGateAni;                       //匣道門動畫控制器
    private Animator     LungAni;                          //肺部動畫控制器

    void Awake()
    {
        //取得
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        GateAni = Gate.GetComponent<Animator>();
        HRAGateAni = HRAGate.GetComponent<Animator>();
        HLAGateAni = HLAGate.GetComponent<Animator>();
        LungAni = GameObject.Find("Box_CO2").GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始值
        isChoice = false;
        isChoice_Part1 = false;
        isChoice_Part2 = false;
        isChoice_Part3 = false;
        isChoice_Part4 = false;
        //isPreQ_Ok = true;
        HRA_num = 0;
        BottleNum = 0;
    }

    public void GetGlucose(RaycastHit hit)
    {
        if(!isChoice)
        {
            if(hit.transform.CompareTag("Glucose"))
            {
                //販賣機領取葡萄糖能量飲
                if(SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    Sounds._Button(1);                                  //領取按鍵音效
                    isChoice =true;
                    if (hit.transform.name=="Q1")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q1....
                        int Q=0;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                    if(hit.transform.name=="Q2")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q2....
                        int Q=1;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                    if(hit.transform.name=="Q3")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q3....
                        int Q=2;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                    if(hit.transform.name=="Q4")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q4....
                        int Q=3;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                    if(hit.transform.name=="Q5")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q5....
                        int Q=4;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                    if(hit.transform.name=="Q6")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q6....
                        int Q=5;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                    if(hit.transform.name=="Q7")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q7....
                        int Q=6;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                    if(hit.transform.name=="Q8")
                    {
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //為Q8....
                        int Q=7;
                        hit.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material=Question[Q];
                        NowSelect=Select[Q];
                        NowSelect.SetActive(true);
                        GetQResult(hit,Q);
                        GetAnser(Q);
                    }
                }
            }
        }
        
    }

    public void ChoiceQ(RaycastHit hit)
    {
        if(isChoice|| isChoice_Part1 || isChoice_Part2 || isChoice_Part3 || isChoice_Part4)
        {
            if(SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                if (hit.transform.name=="A")
                {
                    SelectAnser=1;
                    Judge(hit,SelectAnser);
                }
                if(hit.transform.name=="B")
                {
                    SelectAnser=2;
                    Judge(hit,SelectAnser);
                }
                if(hit.transform.name=="C")
                {
                    SelectAnser=3;
                    Judge(hit,SelectAnser);
                }
                if(hit.transform.name=="D")
                {
                    SelectAnser=4;
                    Judge(hit,SelectAnser);
                }
            }
        }
    }
    
    //BQ(匣道門*1、右心房*3、肺部*1、左心房*2)
    public void GetGate(int Anser)
    {
        if(Anser==0)
        {
            //匣道門
            isChoice_Part1 = true;
        }
        if(Anser==1||Anser==2||Anser==3)
        {
            //右心房通過三尖瓣
            isChoice_Part2 = true;
        }
        if(Anser==4)
        {
            //肺部
            isChoice_Part3 = true;
        }
        if(Anser==5||Anser==6)
        {
            //左心房通過二尖瓣
            isChoice_Part4 = true;
        }
        //現在所選的題目
        NowSelect= B_Select[Anser];
        //現在的答案...第Anser
        NowAnser = B_Question_Anser[Anser];
        //NowJudgeImage暫放
        NowJudgeImage[0] = B_JudgeImage[Anser * 4];
        NowJudgeImage[1] = B_JudgeImage[Anser * 4 + 1];
        NowJudgeImage[2] = B_JudgeImage[Anser * 4 + 2];
        NowJudgeImage[3] = B_JudgeImage[Anser * 4 + 3];
    }

    //Q(葡萄糖*9)
    void GetAnser(int Anser)
    {   
        //現在的答案為...第Anser題答案
        NowAnser=Question_Anser[Anser];
    }

    void GetQResult(RaycastHit _hit, int Anser)
    {
        NowMaterial = _hit.transform.parent.transform.GetChild(2).gameObject;
        //NowJudge暫放
        NowJudgeImage[0] = JudgeImage[Anser * 4];
        NowJudgeImage[1] = JudgeImage[Anser * 4 + 1];
        NowJudgeImage[2] = JudgeImage[Anser * 4 + 2];
        NowJudgeImage[3] = JudgeImage[Anser * 4 + 3];
    }

    //判斷是否正確，給予葡萄糖能量飲
    void Judge(RaycastHit _hit,int _SeletAnser)
    {
        _hit.transform.gameObject.GetComponent<BoxCollider>().enabled=false;
        if(_SeletAnser==NowAnser)
        {
            Sounds._Correct();               //按鍵音效
            //答對了，顯示
            _hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite=NowJudgeImage[_SeletAnser-1];

            //葡萄糖能量飲，1秒突然出現手上
            if (isChoice)
            {
                Invoke("GetCans", 1f);
            }
            //匣道門
            if (isChoice_Part1)
            {
                Invoke("OpenGate",1.5f);
            }
            //右心房
            if(isChoice_Part2)
            {
                HRA_num++;
                if(HRA_num==1)
                {
                    Invoke("Q3Next", 1.5f);
                }
                if(HRA_num==2)
                {
                    Invoke("Q4Next", 1.5f);
                }
                if(HRA_num==3)
                {
                    Invoke("OpenHRADoor", 2f);
                }
            }
            //肺部
            if (isChoice_Part3)
            {
                Invoke("StartLung", 1f);
            }
            //左心房
            if (isChoice_Part4)
            {
                HRA_num++;
                if (HRA_num == 1)
                {
                    Invoke("Q7Next", 1.5f);
                }
                if (HRA_num == 2)
                {
                    Invoke("OpenHLADoor", 2f);
                }
            }
            isChoice = false;
            isChoice_Part1 = false;
            isChoice_Part2 = false;
            isChoice_Part3 = false;
            isChoice_Part4 = false;
        }
        if(_SeletAnser!=NowAnser)
        {
            Sounds._Error();               //按鍵音效
            //答錯了，顯示
            Debug.Log("Error！");
            _hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite=NowJudgeImage[_SeletAnser-1];
        }
        SelectAnser=0;
    }

   
    void GetCans()
    {
        //手上出現葡萄糖能量飲
        BottleNum++;
        //螢幕換成獲得成功
        NowSelect.SetActive(false);
        NowMaterial.GetComponent<MeshRenderer>().material=GetCansScreen;
    }

    void OpenGate()
    {
        //關閉目前題目
        NowSelect.SetActive(false);
        Sounds._AutoDoor(4);                //自動門音效
        //開啟動畫
        GateAni.SetBool("close", false);
        GateAni.SetBool("open", true);

    }

    void Q3Next()
    {
        //關閉目前題目
        NowSelect.SetActive(false);

        //右心房第二題開啟
        B_Select[2].SetActive(true);
        GetGate(2);
    }
    void Q4Next()
    {
        //關閉目前題目
        NowSelect.SetActive(false);

        //右心房第三題開啟
        B_Select[3].SetActive(true);
        GetGate(3);
    }
    void Q7Next()
    {
        //關閉目前題目
        NowSelect.SetActive(false);

        //右心房第三題開啟
        B_Select[6].SetActive(true);
        GetGate(6);
    }

    void OpenHRADoor()
    {
        //關閉目前題目
        NowSelect.SetActive(false);
        Sounds._TrapDoor(6);
        //順利開啟，右心房通往三尖瓣。
        HRA_num = 0;
        HRAGateAni.SetBool("open", true);
    }

    void StartLung()
    {
        //關閉目前題目
        NowSelect.SetActive(false);
        Sounds._Vapor();

        //啟動肺部換氧動畫
        LungAni.SetBool("GoIn", true);
    }

    void OpenHLADoor()
    {
        //關閉目前題目
        NowSelect.SetActive(false);
        Sounds._TrapDoor(10);
        //順利開啟，左心房通往二尖瓣。
        HRA_num = 0;
        HLAGateAni.SetBool("open", true);
    }
}
