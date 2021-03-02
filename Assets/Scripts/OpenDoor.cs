using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {
    
    private GameObject Player;
    [Range(0f,10f)]
    public float Dis_Player;
    public float Dis_Open;
    public bool  OpenSound;
    private Sounds Sounds;
    private Animator ani;

    // Use this for initialization
    void Awake () {
        Player = GameObject.Find("Player");
        Sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
        ani = GetComponent<Animator>();
        OpenSound = false;
    }
    

    // Update is called once per frame
    void Update () {
		Dis_Player= Vector3.Distance(transform.position, Player.transform.position);
        if (Dis_Player < Dis_Open)
        {
            ani.SetBool("open", true);
            isSounds();
        }
        else
        {
            ani.SetBool("open", false);
            OpenSound = false;
        }
    }

    void isSounds()
    {
        if (!OpenSound)
        {
            OpenSound = true;
            Sounds._AutoDoor(0);
            Sounds._AutoDoor(1);
        }
    }
}
