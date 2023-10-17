using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public Node TimePassNode; 

    public IEnumerator TimeRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if (UIManager.Instance.Stage == UIStageType.Puzzle) {
                continue;
            }

            VirtualMachine.Instance.TryExecute(TimePassNode);
            Value value = null;
            TimeValue timeValue = null;
            if (VirtualMachine.Instance.TryGetVar("Time", ref value))
            {
                Debug.Log("Time Value is " + value);
                timeValue = value as TimeValue;
                Debug.Log(timeValue.DaySeconds);
            }
        }
    }
    void Start()
    {
        StartCoroutine(TimeRoutine());
    }
}
