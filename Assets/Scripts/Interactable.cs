using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    public Vector3 m_Offset = Vector3.zero;
    [HideInInspector]
    public Hand m_ActiveHand = null;

    public void ApplyOffset(Transform hand)
    {
        transform.SetParent(hand);
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        transform.localPosition = m_Offset;
        transform.SetParent(null);
    }
    public void CarOffset(Transform hand)
    {

        transform.SetParent(hand);
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        transform.localPosition = m_Offset;
        transform.SetParent(null);
    }
}
