using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    private float m_Gravity = 9.81f;        //重力
    public float m_Sensitivity = 0.1f;      
    public float m_MaxSpeed = 1.0f;         //最大速度
    public float m_RotateIncrement = 90;    //旋轉角度

    public SteamVR_Action_Boolean m_SnapTurnLeft = null;
    public SteamVR_Action_Boolean m_SnapTurnRight = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;

    private float m_Speed = 0.0f;
    
    private CharacterController m_CharacterController = null;
    private Transform m_CameraRig = null;
    private Transform m_Head = null;

    //CharacterController controller;	        //取得CharacterController
    //public  float speed=5f;					//移動速度
    //public float gravity=-9.81f;			//物體重力
    //public float jumpHeight = 3f;           //跳躍高度

    //public Transform groundCheck;			//地板檢查的向量
    //public float groundDistance=0.4f;		//與地板距離(半徑)
    //public LayerMask groundMask;			//地板層

    //Vector3 velocity;						//重力速度
    //bool isGrounded;						//是否碰觸地板


    void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        m_CameraRig = SteamVR_Render.Top().origin;
        m_Head = SteamVR_Render.Top().head;
    }

    void Update()
    {
        //isGrounded=Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        //if(isGrounded &&velocity.y<0)
        //{
        //	velocity.y=-2f;
        //}

        //float x=Input.GetAxis("Horizontal");
        //float z=Input.GetAxis("Vertical");
        //Vector3 move=transform.right*x+transform.forward*z;
        //controller.Move(move*speed*Time.deltaTime);

        //      if (Input.GetButtonDown("Jump") && isGrounded)
        //      {
        //          velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //v=(h*-2*g)平方根
        //      }

        //      //引力
        //velocity.y+=gravity*Time.deltaTime;
        //controller.Move(velocity*Time.deltaTime);
        
        HandleHeight();
        if (!SteamVR_Actions._default.Teleport.GetState(SteamVR_Input_Sources.LeftHand))
            CalculateMovement();
        SnapRotation();
    }

    void HandleHeight()
    {
        //把頭放在當地空間
        float headHeight = Mathf.Clamp(m_Head.localPosition.y, 1, 2);
        m_CharacterController.height = headHeight;

        //減半-碰撞體中心
        Vector3 newCenter = Vector3.zero;
        newCenter.y = m_CharacterController.height / 2;
        newCenter.y += m_CharacterController.skinWidth;

        //移動膠囊在當議空間
        newCenter.x = m_Head.localPosition.x;
        newCenter.z = m_Head.localPosition.z;

        //應用
        m_CharacterController.center = newCenter;
    }

    void CalculateMovement()
    {
        //找出movemonet的方向
        Quaternion orientation = CaulateOrientation();
        Vector3 movement = Vector3.zero;

        //如果不能移動
        if (m_MoveValue.axis.magnitude==0)
            m_Speed = 0;
        
        //Add,clamp
        m_Speed += m_MoveValue.axis.magnitude * m_Sensitivity;
        m_Speed = Mathf.Clamp(m_Speed, -m_MaxSpeed, m_MaxSpeed);

        //方向,重力
        movement += orientation * (m_Speed*Vector3.forward);
        movement.y -= m_Gravity*6 * Time.deltaTime;

        //應用
        m_CharacterController.Move(movement * Time.deltaTime);
    }

    private Quaternion CaulateOrientation()
    {
        float rotation = Mathf.Atan2(m_MoveValue.axis.x, m_MoveValue.axis.y);
        rotation *= Mathf.Rad2Deg;

        Vector3 orientationEuler = new Vector3(0, m_Head.eulerAngles.y+rotation, 0);
        return Quaternion.Euler(orientationEuler);
    }

    private void SnapRotation()
    {
        float snapValue = 0.0f;

        //震度
        

        if (m_SnapTurnLeft.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            snapValue = -Mathf.Abs(m_RotateIncrement);
            SteamVR_Actions._default.Haptic.Execute(0f, 0.1f, 25, 0.1f, (SteamVR_Input_Sources.LeftHand));
        }
            
        if (m_SnapTurnRight.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            snapValue = Mathf.Abs(m_RotateIncrement);
            SteamVR_Actions._default.Haptic.Execute(0f, 0.1f, 25, 0.1f, (SteamVR_Input_Sources.LeftHand));
        }

        transform.RotateAround(m_Head.position, Vector3.up, snapValue);
    }
}
