using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowMove : MonoBehaviour
{
    public GameObject[] arrows;                         //箭頭
    [SerializeField]private List<bool> off_arrows = null;      //關閉箭

    private List<Vector3> originArrow=new List<Vector3>();

    [Range(0.0f, 2.0f)]
    public float Speed=1.5f;

    [Range(0.0f, 2.0f)]
    public float Distance=0.3f;
    int k=0;

    void Start()
    { 
        for(int i=0;i<arrows.Length;i++)
        {
            originArrow.Add(arrows[i].transform.position);
            off_arrows.Add(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        for(k=0;k<arrows.Length;k++){
            if(arrows[k].transform.position.y<=originArrow[k].y-Distance)
            {
                off_arrows[k]=true;
            }
            if(arrows[k].transform.position.y>=originArrow[k].y+Distance)
            {
                off_arrows[k]=false;
            }
            if (off_arrows[k]) 
            {
                arrows[k].transform.DOMoveY(arrows[k].transform.position.y + 0.2f, Speed);
            }
            else
            {
                arrows[k].transform.DOMoveY(arrows[k].transform.position.y - 0.2f, Speed);
            }
        }
    }
}
