using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour
{
    public GameObject m_Pointer;
    public SteamVR_Action_Boolean m_TeleportAction;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool m_HasPosition   = false;
    private bool m_isTeleporting = false;
    private float m_FadeTime = 0.5f;
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
        //Pointer
        if (m_TeleportAction.GetState(m_Pose.inputSource))
        {
            m_HasPosition = UpdatePointer();
            m_Pointer.SetActive(m_HasPosition);
        }

        //Teleport
        if (m_TeleportAction.GetStateUp(m_Pose.inputSource))
            TryTelepport();
    }

    private void TryTelepport()
    {
        //Check for valid position,and if already teleporting
        if (!m_HasPosition || m_isTeleporting)
            return;
        //Get camera rig, and head position
        Transform cameraRig = SteamVR_Render.Top().origin;
        Transform Player = GameObject.Find("Player").transform;
        Vector3 headPosition = SteamVR_Render.Top().head.position;

        //Figure out translation
        Vector3 groundPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
        Vector3 translateVector = m_Pointer.transform.position - groundPosition;

        //Move
        StartCoroutine(MoveRig(Player,cameraRig, translateVector));
    }

    private IEnumerator MoveRig(Transform Player,Transform camerRig,Vector3 translation)
    {
        //Flag
        m_isTeleporting = true;

        //Fade to black
        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        //Apply translation
        yield return new WaitForSeconds(m_FadeTime);
        Player.position += translation;

        //Fade to clear
        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);

        //Debug-flag
        m_Pointer.SetActive(false);
        m_isTeleporting = false;
    }

    private bool UpdatePointer()
    {
        //Ray from the controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        //If it's a hit
        if (Physics.Raycast(ray, out hit,15f) && hit.transform.gameObject.layer==10)
        {
            m_Pointer.transform.position = hit.point;
            return true;
        }

        //If not a hit
        return false;
    }

}
