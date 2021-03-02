using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Console : MonoBehaviour {

    public Sprite[] ItemBody;
    public Sprite[] Body;
    private int Num=0;
    private GameObject Screen_BG;
    private GameObject BodyImage;
    private GameObject ItemBodyImage;

    private Sounds Sounds;
    private ArrowMove ArrowMove;
    private bool isOpen=false;  //電源開啟 
    [HideInInspector]public bool isOpenDoor;    //Level1通關門

    public Animator ani;        //開門動畫

    void Awake()
    {
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        ArrowMove = GameObject.Find("Arrow_Teach").GetComponent<ArrowMove>();
        Screen_BG = gameObject.transform.Find("Screen_BG").gameObject;
        BodyImage=gameObject.transform.Find("Screen_BG").gameObject.transform.Find("Image").gameObject;
        ItemBodyImage = gameObject.transform.Find("Item").gameObject.transform.Find("Body_Item").gameObject;
       
    }

    void Start()
    {
        //初始化
        Screen_BG.SetActive(false); 
        ItemBodyImage.SetActive(false);
        isOpenDoor = false;
    }
    

    void TurnOff()
    {
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))    //按下右-扣板機
        {
            //關閉箭頭
            if (!isOpen)
            {
                ArrowMove.enabled = false;
                ArrowMove.arrows[0].SetActive(false);
                isOpen = true;
                Screen_BG.SetActive(true);
                ItemBodyImage.SetActive(true);
            }
        }
    }
    public void OpenConsole(RaycastHit hit) {
        if (hit.transform.name == "Screen") //如果碰到螢幕
        {
            TurnOff();
        }
        if (isOpen)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name == "Right")
            {
                if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    if (Num < Body.Length-1)
                    {
                        Sounds._Button(0);
                        Num++;
                    }
                    
                    if (Num == Body.Length-1)
                    {
                        if (!isOpenDoor)
                        {
                            //如果點到最後一個部位可以開啟
                            isOpenDoor = true;
                            Level1_Win();
                        }
                    }
                }
                ShowBody(Num);
            }
            else if (hit.transform.name == "Left")
            {
                if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    if (Num > 0)
                    {
                        Sounds._Button(0);
                        Num--;
                    }
                }
                ShowBody(Num);
            }
            
        }
    }

    void ShowBody(int Num)   ///控制台管理
    {
        BodyImage.GetComponent<SpriteRenderer>().sprite = Body[Num];
        ItemBodyImage.GetComponent<SpriteRenderer>().sprite = ItemBody[Num];
    }

    //第一關通過，通關門開啟
    void Level1_Win()
    {
        ani.SetBool("character_nearby", true);
        Sounds._Door(0);
    }
}
