using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 변수의 정보를 저장하는 노드
/// </summary>
public class Var : Node {
    public string VerName { get { return varName; } }

    [SerializeField]
    private string varName;

    [SerializeField]
    private Value value;

    public override Value Visit(IVirtualMachine machine) {
        if (machine.TryGetVar(varName, ref value)) {
            return value;
        }

        return null;
    }

    public bool Set(IVirtualMachine machine, Value value) {
        if (!machine.TryGetVar(varName, ref this.value)) {
            var instance = new GameObject("@" + varName);
            DontDestroyOnLoad(instance);
            var clone = value.Clone(instance);
            this.value = clone;

            return machine.TrySetVar(varName, this.value);
        }

        this.value.Set(value);
        return true;
    }

    public bool Add(IVirtualMachine machine, Value value) {
        if (!machine.TryGetVar(varName, ref this.value)) {
            var instance = new GameObject("@" + varName);
            DontDestroyOnLoad(instance);
            var clone = value.Clone(instance);
            this.value = clone;

            return machine.TrySetVar(varName, this.value);
        }

        this.value.Add(value);
        return true;
    }
}
