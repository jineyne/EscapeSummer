using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Animator))]
public class JellyFish : MonoBehaviour
{
    public bool PlayerOn = false;
    private Animator animator;
    // Start is called before the first frame update

    public bool IsDay()
    {
        Value value = null;
        TimeValue timevalue = null;
        if (VirtualMachine.Instance.TryGetVar("Time", ref value))
        {
            timevalue = value as TimeValue;
        }
       
        return timevalue.IsDay;
    }
    public bool IsNight()
    {
        Value value = null;
        TimeValue timevalue = null;
        if (VirtualMachine.Instance.TryGetVar("Time", ref value))
        {
            timevalue = value as TimeValue;
        }
        return timevalue.IsNight;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        this.UpdateAsObservable()
           .Where(_ => IsDay())
           .Subscribe(_ => { gameObject.GetComponent<BoxCollider2D>().enabled = false; animator.SetBool("Up", false); })
           .AddTo(gameObject);

        this.UpdateAsObservable()
           .Where(_ => IsNight() || PlayerOn)
           .Subscribe(_ => { gameObject.GetComponent<BoxCollider2D>().enabled = true; animator.SetBool("Up", true); })
           .AddTo(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerOn = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        PlayerOn = false;
    }
}
