using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMInitHelper : MonoSingleton<VMInitHelper> {
    public List<Node> InitNodes;

    void Awake() {
        VirtualMachine.Instance.CleanUp();

        foreach (var node in InitNodes) {
            VirtualMachine.Instance.TryExecute(node);
        }
    }
}
