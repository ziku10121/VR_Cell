using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    public SteamVR_Action_Boolean m_GrabAction = null;
    public SteamVR_Action_Boolean m_CheckInf = null;
    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    public Interactable m_CurrentInteractable = null;
    public List<Interactable> m_ContactInteractables = new List<Interactable>();

    public GameObject MouseControl;

    void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    void Update()
    {
        //射線開關
        if (SteamVR_Actions._default.CheckInf.GetState(SteamVR_Input_Sources.RightHand)&& m_CheckInf.GetStateDown(m_Pose.inputSource))
        {
            if (MouseControl.GetComponent<MouseControl>().enabled)
            {
                MouseControl.GetComponent<MouseControl>().enabled = false;
                GameObject Laser = transform.Find("Laser(Clone)").gameObject;
                Laser.SetActive(false) ;
            }
            else
            {
                MouseControl.GetComponent<MouseControl>().enabled = true;
            }
        }
        
        //Down
        if (m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            //Debug.Log(m_Pose.inputSource + "Trigger Down");
            Pickup();
        }
        //Up
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            //Debug.Log(m_Pose.inputSource + "Trigger Up");
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer!=9)
            return;
        m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 9)
            return;
        m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    public void Pickup()
    {
        //最近可交互對象
        m_CurrentInteractable = GetNearestInteractable();

        //Null檢查
        if (!m_CurrentInteractable)
            return;

        //已經持有,檢查
        if (m_CurrentInteractable.m_ActiveHand)
            m_CurrentInteractable.m_ActiveHand.Drop();

        //位置
        if(m_CurrentInteractable.CompareTag("Body") || m_CurrentInteractable.CompareTag("Box_CO2"))
            m_CurrentInteractable.ApplyOffset(transform);
        if (m_CurrentInteractable.gameObject.name== "Pushcart")
            m_CurrentInteractable.CarOffset(transform);
        m_CurrentInteractable.transform.position = transform.position;

        //關節附上
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        //設定主動手
        m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {
        //Null 檢查
        if (!m_CurrentInteractable)
            return;

        //應用施加速度
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity();
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();

        //關節分離
        m_Joint.connectedBody = null;

        //清除
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    //最近可交互對象
    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable interactable in m_ContactInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude; //(手距位置-物體位置)的平方=距離

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
    }
}
