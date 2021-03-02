using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PuzzleGame : MonoBehaviour
{
    public GameObject Human;                    //人體構造
    public GameObject[] Pic_Components = null;  //放拼圖元件
    private int ComponentsSize = 0;             //放拼圖元件Count
    public GetPic[] Pic_Group = null;           //拼圖的容器
    private int GroupSize = 0;                  //拼圖的容器Count
    public Material OrigilRangeColor;           //淺色
    public Material InRangeColor;               //到範圍的顏色

    private static int Good_Count;                 //答對次數

    public bool inGame = false;                //是否在遊戲階段
    [SerializeField]private bool isBingo = false;               //是否貼圖正確
    public  bool isInfShow = false;              //器官資訊
    public GameObject[] ShowHide;           //機能按鈕群組
    
    public Hand Right;
    public Hand Left;
    public Console Console;
    private ArrowMove ArrowMove;
    private Sounds Sounds;

    void Awake()
    {
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        ArrowMove = GameObject.Find("Arrow_Teach").GetComponent<ArrowMove>();
    }

    void Start()
    {
        //對應器官容器換半透明材質球
        for (int i = 0; i < GroupSize; i++)
        {
            Pic_Group[i].GetComponent<Renderer>().material = OrigilRangeColor;
        }

        //事先取得陣列數量，避免迴圈時造成效能損耗
        ComponentsSize = Pic_Components.Length;
        GroupSize = Pic_Group.Length;

        //起始遊戲
        StartGame();
    }

    public void StartGame()
    {
        //答對次數歸零
        Good_Count = 0;
        //抓取器官容器與對應器官容器 都開啟
        for (int i = 0; i < ComponentsSize; i++)
        {
            Pic_Components[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < GroupSize; i++)
        {
            Pic_Group[i].gameObject.SetActive(true);
        }
        //準備就緒開始遊戲
        inGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame) //遊戲是否進行
        {
            if (Right.m_CurrentInteractable != null && Right.m_CurrentInteractable.CompareTag("Body"))
            {
                Puzzle_body(Right.m_CurrentInteractable.gameObject);
            }
            if (Left.m_CurrentInteractable != null && Left.m_CurrentInteractable.CompareTag("Body"))
            {
                Puzzle_body(Left.m_CurrentInteractable.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            inGame = false;                         
            ShowHide[0].SetActive(true);            
            ShowHide[1].SetActive(true);            
            ShowHide[2].SetActive(false);           
            ArrowMove.enabled = true;
            ArrowMove.arrows[0].SetActive(true);
            this.enabled = false;
        }
    }

    public void Puzzle_body(GameObject GrabObj)
    {
        BodyInf(GrabObj);
        int i = 0;
        //VR版
        if (SteamVR_Actions._default.GrabGrip.GetState(SteamVR_Input_Sources.LeftHand)
            || SteamVR_Actions._default.GrabGrip.GetState(SteamVR_Input_Sources.RightHand))
        {
            //讓物件靠近自己
            for (i = 0; i < GroupSize; i++)
            {
                //如果容器指定的名稱與滑鼠選取的名稱相同
                if (Pic_Group[i].GetName == GrabObj.name)
                {
                    //取得距離
                    float GetDistance = Vector3.Distance(GrabObj.transform.position, Pic_Group[i].transform.position);
                    //Debug.Log("距離" + GetDistance);
                    //距離小於0.05會呈現顏色提示擺放位子
                    if (GetDistance < 0.05f)
                    {
                        Pic_Group[i].GetComponent<Renderer>().material = InRangeColor;
                        isBingo = true;
                    }
                    else
                    {
                        Pic_Group[i].GetComponent<Renderer>().material = OrigilRangeColor;
                        isBingo = false;
                    }
                }
            }
        }
        if (SteamVR_Actions._default.GrabGrip.GetStateUp(SteamVR_Input_Sources.LeftHand)
            || SteamVR_Actions._default.GrabGrip.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            if (isBingo == true)
            {
                for (i = 0; i < GroupSize; i++)
                {
                    //如果容器指定的名稱與滑鼠選取的名稱相同
                    if (Pic_Group[i].GetName == GrabObj.name)
                    {
                        Left.m_ContactInteractables.Remove(Right.m_CurrentInteractable.GetComponent<Interactable>());
                        Right.m_ContactInteractables.Remove(Right.m_CurrentInteractable.GetComponent<Interactable>());
                        //當距離小於0.3判定為正確把拼圖放到對的位置
                        Pic_Group[i].Good_Answer(GrabObj.gameObject);
                        Good_Count += 1;
                        Debug.Log(Good_Count);
                        isBingo = false;

                        //當答對數量等於正確容器數量
                        if (Good_Count == Pic_Group.Length)
                        {
                            inGame = false;                         //遊戲結束，可以開啟控制台
                            ShowHide[0].SetActive(true);            //主動脈開啟
                            ShowHide[1].SetActive(true);            //靜動脈開啟
                            ShowHide[2].SetActive(false);           //器官拼圖關閉
                            //Human.transform.position = new Vector3(Human.transform.position.x, 0.2f, Human.transform.position.z);
                            //Human.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
                            

                            //開啟箭頭
                            ArrowMove.enabled = true;
                            ArrowMove.arrows[0].SetActive(true);
                            this.enabled = false;
                        }
                    }
                }
            }
        }
    }

    void BodyInf(GameObject GrabObj)
    {
        if (GrabObj.transform.CompareTag("Body"))
        {
            if (SteamVR_Actions._default.CheckInf.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                if (isInfShow == false)
                {
                    GrabObj.transform.GetChild(0).gameObject.SetActive(true);
                    isInfShow = true;
                }
                else
                {
                    GrabObj.transform.GetChild(0).gameObject.SetActive(false);
                    isInfShow = false;
                }
            }
        }
    }
}


