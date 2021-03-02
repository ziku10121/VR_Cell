using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPic : MonoBehaviour {
    
    public string GetName= "";   //設定答案名稱

    public void Good_Answer(GameObject GetAnswer)
    {
        GetComponent<Renderer>().materials = GetAnswer.GetComponent<Renderer>().materials;
        Debug.Log(this.name);
        GetAnswer.SetActive(false);
        Destroy(GetAnswer);
    }
    
}
