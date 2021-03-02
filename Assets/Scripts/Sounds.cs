using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

    private AudioSource BGMaudio;       //背景聲音來源
    public AudioSource[] audio;          //各種聲音來源(0:Play(1)、1:Player(2)、2:Level_Door、3:Portal、4:匣道門、5:右心房收縮、6:右心房到三尖瓣、7:鐘聲、8:右心室到肺動脈、9:肺部處理、10:左心房到二尖瓣、11:左心室到主動脈，12:電梯)
    public AudioSource[] AIaudio;        //AI聲音來源(0:Teach_RedBloodCell、1:Teach_WhiteBloodCell、2:Teach_Glucose、3:Red、4:White、5:JellyFishGirl)


    private AudioClip Doordrop;      //門掉落聲音效
    private AudioClip Shuttle;       //穿梭門音效
    private AudioClip Door;          //普通門音效
    private AudioClip CloseDoor;     //普通關門音效
    private AudioClip AutoDoor;      //自動門音效
    private AudioClip TrapDoor_Open; //陷阱開門音效
    private AudioClip TrapDoor_Close;//陷阱關門音效
    private AudioClip Button;        //按鈕音效
    private AudioClip Correct;       //正確音效
    private AudioClip Error;         //錯誤音效
    private AudioClip Church_Bell;   //鐘聲音效
    private AudioClip Vapor;   //蒸氣音效(肺部二氧轉氧氣)

    public AudioClip[] BGM = new AudioClip[4];                                  //背景音樂
    private AudioClip[] Red_Teach = new AudioClip[5];                           //紅血球箱子教學音效
    private AudioClip[] White_Teach = new AudioClip[6];                         //白血球射擊教學音效
    [SerializeField] private AudioClip[] Glucose_Teach = new AudioClip[5];      //紅血球能量飲教學音效

    public AudioClip[] Monster_Attack = new AudioClip[4];      //怪物攻擊音效(0:腸炎弧菌、綠膿桿菌；1:仙人掌桿菌；2:肺炎鏈球菌、金黃色葡萄球菌；3:海獸胃腺蟲)
    public AudioClip[] Monster_Move = new AudioClip[4];      //怪物移動音效(0:腸炎弧菌、綠膿桿菌；1:仙人掌桿菌；2:肺炎鏈球菌、金黃色葡萄球菌；3:海獸胃腺蟲)
    public AudioClip[] Monster_Die = new AudioClip[2];      //怪物死亡音效(0:腸炎弧菌、綠膿桿菌、仙人掌桿菌、肺炎鏈球菌、金黃色葡萄球菌；1:海獸胃腺蟲)

    private AudioClip pop_window;    //跳出視窗
    private AudioClip background_mic;//背景音樂

    private AudioClip hurt;          //受傷

    void Awake() {
        //BGM
        BGMaudio        = GetComponent<AudioSource>();
        BGM[0]          = Resources.Load<AudioClip>("Sounds/BGM/Play");
        BGM[1]          = Resources.Load<AudioClip>("Sounds/BGM/Heart");
        BGM[2]          = Resources.Load<AudioClip>("Sounds/BGM/Battle");
        BGM[3]          = Resources.Load<AudioClip>("Sounds/BGM/Room");
        //其他
        Doordrop        = Resources.Load<AudioClip>("Sounds/Doordrop");
        Shuttle         = Resources.Load<AudioClip>("Sounds/Shuttle");
        Door            = Resources.Load<AudioClip>("Sounds/Door");
        CloseDoor       = Resources.Load<AudioClip>("Sounds/CloseDoor");
        AutoDoor        = Resources.Load<AudioClip>("Sounds/AutoDoor");
        TrapDoor_Open   = Resources.Load<AudioClip>("Sounds/TrapDoor_Open");
        TrapDoor_Close  = Resources.Load<AudioClip>("Sounds/TrapDoor_Close");
        Button          = Resources.Load<AudioClip>("Sounds/Button");
        Correct         = Resources.Load<AudioClip>("Sounds/Correct");
        Error           = Resources.Load<AudioClip>("Sounds/Error");
        Church_Bell     = Resources.Load<AudioClip>("Sounds/Church_Bell");
        Vapor           = Resources.Load<AudioClip>("Sounds/Vapor");

        //NPC
        for (int i = 0; i < Red_Teach.Length; i++)
        {
            Red_Teach[i] = Resources.Load<AudioClip>("Sounds/AISay/Red_Teach" + (i + 1));
        }
        for (int i = 0; i < White_Teach.Length; i++)
        {
            White_Teach[i] = Resources.Load<AudioClip>("Sounds/AISay/White_Teach" + (i + 1));
        }
        for (int i = 0; i < Glucose_Teach.Length; i++)
        {
            Glucose_Teach[i] = Resources.Load<AudioClip>("Sounds/AISay/Glucose_Teach" + (i + 1));
        }
        //Enemy
        for (int i = 0; i < Monster_Attack.Length; i++)
        {
            Monster_Attack[i] = Resources.Load<AudioClip>("Sounds/Monster/Monster_Attack" + (i + 1));
        }
        for (int i = 0; i < Monster_Move.Length; i++)
        {
            Monster_Move[i] = Resources.Load<AudioClip>("Sounds/Monster/Monster_Move" + (i + 1));
        }
        for (int i = 0; i < Monster_Die.Length; i++)
        {
            Monster_Die[i] = Resources.Load<AudioClip>("Sounds/Monster/Monster_Die" + (i + 1));
        }

        hurt        = Resources.Load<AudioClip>("Sounds/hurt");
    }

    void Start()
    {
        //初始化
        _BGM(BGM[0]);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _BGM(BGM[1]);
        }
    }
    public void _BGM(AudioClip NextBGM)
    {
        if (BGMaudio.clip == null)
        {
            BGMaudio.clip = BGM[0];
            BGMaudio.Play();
        }
        else
        {
            StartCoroutine(FadeIn(NextBGM));
        }
    }
    IEnumerator FadeIn(AudioClip NextBGM)
    {
        float _Volume = BGMaudio.volume;
        while (_Volume > 0f)
        {
            _Volume -= Time.deltaTime * 1f;
            BGMaudio.volume = _Volume;
            Debug.Log("FIFIFFIFI");
            yield return new WaitForSeconds(0.1f);
        }
        BGMaudio.clip = NextBGM;
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        BGMaudio.Play();
        float _Volume = BGMaudio.volume;
        while (_Volume < 0.15f)
        {
            _Volume += Time.deltaTime * 1f;
            BGMaudio.volume = _Volume;
            Debug.Log("FOFOFOFOFO");
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    //---------------------------------------------------
    public void _Doordrop()
    {
        //播放傳送門掉下音效
        audio[3].clip = Doordrop;
        audio[3].Play();
    }
    public void _Shuttle()
    {
        //播放傳送門音效
        audio[1].clip = Shuttle;
        audio[1].volume = 0.5f;
        audio[1].Play();
    }

    public void _Door(int num)
    {
        //播放回答錯誤音效
        audio[num].clip = Door;
        audio[num].Play();
    }
    public void _CloseDoor(int num)
    {
        //播放回答錯誤音效
        audio[num].clip = CloseDoor;
        audio[num].Play();
    }
    public void _AutoDoor(int num)
    {
        //播放回答錯誤音效
        audio[num].clip = AutoDoor;
        audio[num].Play();
    }
    public void _TrapDoor(int num)
    {
        //播放回答錯誤音效
        audio[num].clip = TrapDoor_Open;
        audio[num].Play();
    }
    public void _CloseTrapDoor(int num)
    {
        //播放回答錯誤音效
        audio[num].clip = TrapDoor_Close;
        audio[num].Play();
    }

    public void _Button(int num)
    {
        //播放點擊音效
        audio[num].clip = Button;
        audio[num].volume = 0.35f;
        audio[num].Play();
    }
    public void _Correct()
    {
        //播放回答正確音效
        audio[1].clip = Correct;
        audio[1].Play();
    }
    public void _Error()
    {
        //播放回答錯誤音效
        audio[1].clip = Error;
        audio[1].Play();
    }
    public void _Church_Bell()
    {
        //播放敲鐘音效
        audio[7].clip = Church_Bell;
        audio[7].Play();
    }
    public void _Vapor()
    {
        //播放肺部轉換成氧氣音效
        audio[9].clip = Vapor;
        audio[9].Play();
    }

    //---------------------------------------------------
    public void Red_TeachSay(int Clipnum)
    {
        //播放紅血球箱子教學音效
        AIaudio[0].clip = Red_Teach[Clipnum];
        AIaudio[0].Play();
    }

    public void White_TeachSay(int Clipnum)
    {
        //播放紅血球箱子教學音效
        AIaudio[1].clip = White_Teach[Clipnum];
        AIaudio[1].Play();
    }

    public void Glucose_TeachSay(int Clipnum)
    {
        //播放紅血球箱子教學音效
        AIaudio[2].clip = Glucose_Teach[Clipnum];
        AIaudio[2].Play();
    }
}
