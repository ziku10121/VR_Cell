using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRay : MonoBehaviour
{
    public GameObject Cell_Inf;

    private GameObject HMP;

    private void Awake()
    {
        HMP = Cell_Inf.transform.Find("Canvas").transform.gameObject;
    }

    private void Start()
    {
        Cell_Inf.GetComponent<SpriteRenderer>().enabled = false;
        HMP.GetComponent<Canvas>().enabled = false;
        this.enabled = false;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Ray myRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        if (Physics.Raycast(myRay, out hit, 10))
        {
            if (hit.transform.name == "Controller (left)")
            {
                Teach_Glucose.isLookInf = true;
                Cell_Inf.GetComponent<SpriteRenderer>().enabled = true;
                HMP.GetComponent<Canvas>().enabled = true;
            }
            else
            {
                Cell_Inf.GetComponent<SpriteRenderer>().enabled = false;
                HMP.GetComponent<Canvas>().enabled = false;
            }
        }
    }
}
