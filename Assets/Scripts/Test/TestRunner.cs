using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRunner : MonoBehaviour {
    public Node LoopNode;
    public Node Set12AMNode;
    public Node Set12PMNode;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(TimeRoutine());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            VirtualMachine.Instance.TryExecute(Set12AMNode);
            Value value = null;
            if (VirtualMachine.Instance.TryGetVar("Time", ref value)) {
                Debug.Log("Time Value is " + value);
                var time = value as TimeValue;
                Debug.Log(time.DaySeconds);
            }
        } else if (Input.GetKeyDown(KeyCode.S)) {
            VirtualMachine.Instance.TryExecute(Set12PMNode);
            Value value = null;
            if (VirtualMachine.Instance.TryGetVar("Time", ref value)) {
                Debug.Log("Time Value is " + value);
                var time = value as TimeValue;
                Debug.Log(time.DaySeconds);
            }
        }
    }

    public IEnumerator TimeRoutine() {
        Value value = null;

        while (true) {
            yield return new WaitForSeconds(1.0f);

            VirtualMachine.Instance.TryExecute(LoopNode);
            if (VirtualMachine.Instance.TryGetVar("Time", ref value)) {
                Debug.Log("Time Value is " + value);
                var time = value as TimeValue;
                Debug.Log(time.DaySeconds);
            }
        }
    }
}
