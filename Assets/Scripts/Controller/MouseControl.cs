using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Valve.VR;

public class MouseControl : MonoBehaviour
{
    public enum LevelState
    {        
        Level1,
        Level2
    }
    public LevelState currentLevel;

    public int WeaponMode=0;                                //武器模式
    [SerializeField] private float forwardDis;
    public GameObject Gun;                                  //抗生素槍
    public GameObject Can;                                  //能量飲
    private Transform    Gun_muzzle;                       //槍口

    public GameObject hitBall;                                //瞄準點
    public Vector3 hitPoint;                                     //瞄準點位子
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;

    private PuzzleGame PuzzleGamee;
    public Console Console;

    private Tech_RedBloodCell   Teach_RedBloodCell;
    public  Teach_WhiteBloodCell Teach_WhiteBloodCell;
    private Teach_Glucose Teach_Glucose;
    private QuestionController   QuestionController;
    private ArrowMove ArrowMove;
    private Sounds Sounds;
    public  Animator ElevatorAni;

    public int BottleNum { get; private set; }

    void Awake()
    {
        PuzzleGamee = GameObject.Find("Player").GetComponent<PuzzleGame>();
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        ArrowMove = GameObject.Find("Arrow_Teach").GetComponent<ArrowMove>();
        Teach_RedBloodCell = GameObject.Find("NPC").transform.Find("Teach_RedBloodCell").transform.GetComponent<Tech_RedBloodCell>();
        Teach_Glucose = GameObject.Find("NPC").transform.Find("Teach_Glucose").transform.GetComponent<Teach_Glucose>();
        QuestionController = GameObject.Find("QuestionController").transform.GetComponent<QuestionController>();
        Gun_muzzle = Gun.transform.Find("muzzle").transform;
        Gun.SetActive(false);
        Can.SetActive(false);
    }

    void Start()
    {
        laser = Instantiate(laserPrefab, transform);
        laserTransform = laser.transform;
        //起始遊戲
        currentLevel=LevelState.Level1;
    }

    private void ShowLaser(RaycastHit hit)
    {
        laserTransform.position = Vector3.Lerp(transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
        laser.SetActive(true);
    }

    private void GunLaser(RaycastHit hit)
    {
        laserTransform.position = Vector3.Lerp(Gun_muzzle.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
        laser.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentLevel)
        {
            case LevelState.Level1:
                Puzzle();
                break;
            case LevelState.Level2:
                SelectedWeapon();
                if (WeaponMode == 0)
                {
                    HandStatus();
                }
                if (WeaponMode == 1)
                {
                    //抗生素槍武器狀態(下鍵鍵盤)
                    GunShoot();
                }
                if (WeaponMode == 2)
                {
                    //葡萄糖狀態(右鍵鍵盤)
                    
                }
                if (WeaponMode == 3)
                {
                    //血液循環圖狀態(左鍵鍵盤)
                    if (Teach_RedBloodCell.isGetMap)
                    {
                        //獲得血液循環圖後...

                    }
                }
                break;
        }
    }

    #region 選擇右手武器
    void SelectedWeapon()
    {
        if (SteamVR_Actions._default.SwitchHand.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            //按下1，手
            WeaponMode = 0;
            Gun.SetActive(false);
            Can.SetActive(false);
        }
        if (SteamVR_Actions._default.SwitchGun.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            //按下2，抗生素槍
            if (Teach_WhiteBloodCell.isGetArms)
            {
                //擁有抗生素槍
                WeaponMode = 1;
                Gun.SetActive(true);
                Can.SetActive(false);
            }
        }
        if (SteamVR_Actions._default.SwitchGlucose.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            
            //按下3，葡萄糖
            if (BottleNum != 0)
            {
                WeaponMode = 2;
                Can.SetActive(true);
                Gun.SetActive(false);
            }
        }
        if (SteamVR_Actions._default.SwitchMap.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            //按下4，血液循環圖
            WeaponMode = 3;
        }
    }
    #endregion

    private void Puzzle()
    {
        Ray myRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(transform.position,transform.forward*100,Color.red);
        //往前射線10以內
        if (Physics.Raycast(myRay, out hit, 100))
        {
            hitPoint = hit.point;
            hitBall.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            hitBall.SetActive(true);
            ShowLaser(hit);                 //啟動雷射
        }
        else
        {
            hitBall.SetActive(false);
            laser.SetActive(false);
        }

        if (PuzzleGamee.inGame != true)        //如果不在遊戲(拼完圖)
        {
            Console.OpenConsole(hit);             //人體介紹控制器
            if (Console.isOpenDoor)                 //如果資訊欄看到底可以進行第二關
            {
                currentLevel = LevelState.Level2;
            }
        }
    }

    private void HandStatus()
    {
        //Ray
        Ray HandRay = new Ray(transform.position, transform.forward);
        RaycastHit Handhit;
        if (Physics.Raycast(HandRay, out Handhit, 80))
        {
            hitPoint = Handhit.point;
            hitBall.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            hitBall.SetActive(true);
            ShowLaser(Handhit);                 //啟動雷射
        }
        else
        {
            hitBall.SetActive(false);
            laser.SetActive(false);
        }

        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("Pinch");
            //Hand狀態(左鍵鍵盤)
            if (Handhit.transform.name == "Red" || Handhit.transform.name == "White" || Handhit.transform.name == "Platelet")
            {
                //如果是知識聊天
                Handhit.transform.gameObject.GetComponent<Chat_Knowledge>().Chat_Know(Handhit);
            }
            if (Handhit.transform.name == "Up")
            {
                //按樓梯升降梯
                ElevatorAni.SetBool("up", true);
                Invoke("OpenElevator", 1f);
                Invoke("ElevatorOK", 4.2f);
            }
            if (Handhit.transform.name == "Trigger")
            {
                //關掉怪物卡片資訊
                GameObject MonsterCard = Handhit.transform.parent.gameObject;
                MonsterCard.SetActive(false);
                //10秒後自動關閉
                StartCoroutine(CloseCard(MonsterCard));
            }
        }
        
        Teach_RedBloodCell.ChatNext(Handhit);          //聊天下一頁觸碰(箱子教學)
        Teach_RedBloodCell.PickUp(Handhit);             //檢箱子、推車動作
        Teach_WhiteBloodCell.TeachArms(Handhit);    //槍枝訓練
        Teach_Glucose.ChatNext(Handhit);                //聊天下一頁觸碰(能量飲)
        QuestionController.GetGlucose(Handhit);        //獲得能量飲
        QuestionController.ChoiceQ(Handhit);            //獲得能量飲
    }

    private void GunShoot()
    {
        //Ray
        Ray GunRay = new Ray(Gun_muzzle.position, Gun_muzzle.forward);
        RaycastHit Gunhit;
        if (Physics.Raycast(GunRay, out Gunhit, 100))
        {
            hitPoint = Gunhit.point;
            hitBall.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            hitBall.SetActive(true);
            GunLaser(Gunhit);                 //啟動雷射
        }
        else
        {
            hitBall.SetActive(false);
            laser.SetActive(false);
        }

        if (Gunhit.transform.CompareTag("Enemy"))
        {
            if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                Gunhit.transform.gameObject.GetComponent<EnemyController>().MonsterHurt(1);
            }
        }
    }

    void OpenElevator()
    {
        ElevatorAni.SetBool("open", true);
    }

    void ElevatorOK()
    {
        transform.parent.gameObject.transform.parent = null;
        transform.parent.gameObject.GetComponent<PlayerMovement>().enabled = true;
    }

    IEnumerator CloseCard(GameObject _MonsterCard)
    {
        yield return new WaitForSeconds(10f);       //等待10秒
        Debug.Log("BBB"+_MonsterCard.active);
        if (_MonsterCard.active == true)
        {
            Debug.Log("AAA");
            //如果怪物卡片狀態還開起...
            _MonsterCard.gameObject.SetActive(false);
        }        
    }
}