using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    public Sprite[] TeachChat_Red;          //教學紅血球
    public Sprite[] TeachChat_White;
    public Sprite[] TeachChat_Glucose;
    public Sprite[] KnowChat_Red;
    public Sprite[] KnowChat_White;
    public Sprite[] KnowChat_Platelet;



    // Start is called before the first frame update
    public void NPC_RedTeach(Sprite Chat_Sprite,int i)
    {
        Debug.Log(Chat_Sprite);
        Chat_Sprite=TeachChat_Red[i];
        Debug.Log("CHAT:"+Chat_Sprite);
        Debug.Log("A");
    }
}
