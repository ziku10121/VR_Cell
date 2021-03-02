using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public bool isObject;   //判斷推車上是否有物件

    void FixedUpdate()
    {
        //固定判斷推車上是否有物件
        if(gameObject.transform.childCount==0)
        {
            isObject=false;
            Invoke("OpenBoxTrigger",2f);
        }
        else if(gameObject.transform.childCount>0)
        {
            isObject=true;
            this.gameObject.GetComponent<Collider>().enabled=false; //推車上關閉觸發器
        }
    }
    public void OnTriggerEnter(Collider e)
    {
        if(!isObject)
        {
            //推車上沒有物件時
            if (e.CompareTag("Box"))
            {
                //如果是Box的Tag(O2、CO2)
                e.transform.GetComponent<BoxCollider>().isTrigger = true;
                e.transform.GetComponent<Rigidbody>().isKinematic = true;
                e.transform.parent = this.transform;                    //Box存在BoxTriiger子物件
                e.transform.localPosition= new Vector3(0,-0.5f,0f);
                e.transform.localEulerAngles = new Vector3(0, 90, 0);
                e.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
        }
        
    }

    //2秒緩衝開啟觸碰器
    void OpenBoxTrigger()
    {
        gameObject.GetComponent<Collider>().enabled=true;
    }
}
