using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Gate : MonoBehaviour
{
    public UnityEvent GateEvent;

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GateEvent.Invoke();
        }
    }
}
