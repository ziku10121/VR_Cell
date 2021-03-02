using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungChange : MonoBehaviour
{
    public Material O2_Material;
    public GameObject O2;
    private Sounds Sounds;
    private ArrowMove ArrowMove;
    //蒸氣粒子特效
    public GameObject SmokeParticle;
    public GameObject SmokeParticle1;
    public GameObject SmokeParticle2;

    private Animator CO2_Ani;

    public bool isLung;

    void Awake()
    {
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        CO2_Ani = GetComponent<Animator>();
        ArrowMove = GameObject.Find("Arrow_Teach").GetComponent<ArrowMove>();
    }

    void Start()
    {
        //初始值
        isLung = false;
    }

    private void Update()
    {
    }

    public void OnTriggerEnter(Collider e)
    {
        if(e.name== "BoxLungTrigger")
        {
            ArrowMove.arrows[5].SetActive(false);                       //關閉箭頭
            isLung = true;
            this.transform.parent = null;
            this.transform.parent = this.transform;                    //Box存在BoxTriiger子物件
            this.transform.position = new Vector3(360.66f,18.195f, -83.21f);
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.transform.localScale = new Vector3(1, 1, 1);
            //如果是二氧化碳
            GetComponent<Animator>().enabled = true;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void GasGet()
    {
        GetComponent<MeshRenderer>().material = O2_Material;
    }

    public void ChangeBox()
    {
        SmokeParticle.SetActive(false);
        SmokeParticle1.SetActive(false);
        SmokeParticle2.SetActive(false);
        Destroy(this.gameObject);
        O2.SetActive(true);
    }

    public void StartFX()
    {
        SmokeParticle.SetActive(true);
        SmokeParticle1.SetActive(true);
        SmokeParticle2.SetActive(true);
        SmokeParticle.GetComponent<ParticleSystem>().Play();
        SmokeParticle1.GetComponent<ParticleSystem>().Play();
        SmokeParticle2.GetComponent<ParticleSystem>().Play();
    }
}
