using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Teach_EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float stoppingDis = 5f;
    public GameObject Player;
    private Transform target;

    [Range(1.0f, 20.0f), SerializeField]
    private float _moveDuration = 5.0f;

    private Animator EnemyAni;
    public  bool isPlotMode=true;
    // Start is called before the first frame update
    void Awake()
    {
        //Player = GameObject.Find("Player (1)");
        EnemyAni = GetComponent<Animator>();
    }

    void Start()
    {
        target = Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlotMode)
        {
            //劇情模式開啟
            Plot_State();
        }
    }
    void Plot_State()
    {
        //劇情狀態
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            //面相敵人
            FaceTarget();
            //距離小於看到範圍(追逐狀態)
            EnemyAni.SetBool("Run", true);
            transform.DOMove(target.position, _moveDuration).SetDelay(1f);
            if (distance <= stoppingDis)
            {
                //攻擊範圍
                EnemyAni.SetBool("Run", false);
                transform.DOPause();
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }


}
