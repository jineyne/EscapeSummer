using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarValue : Value {
    [SerializeField]
    private string name;

    [SerializeField]
    private Value value;

    public override Value Visit(IVirtualMachine machine) {
        if (value == null) {
            machine.TryGetVar(name, ref value);
        }

        return value;
    }

    public override void Set(Value value) {
        if (value == null) {
            this.value = value;
        }

        this.value.Set(value);
    }

    public override void Add(Value value) {
        if (value == null) {
            this.value = value;
        } else {
            this.value.Add(value);
        }
    }

    public override Value Clone(GameObject target) {
        var component = target.AddComponent<VarValue>();
        component.name = name;
        component.value = value;

        return component;
    }

    public override string ToString() {
        return name.ToString();
    }
}
