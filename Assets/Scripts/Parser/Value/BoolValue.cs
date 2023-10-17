using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 정수형을 나타내는 값
/// </summary>
public class BoolValue : Value {
    public bool Value {
        get { return value; }
        set { this.value = value; }
    }

    [SerializeField]
    private bool value;

    public override Value Visit(IVirtualMachine machine) {
        return this;
    }

    public override void Set(Value value) {
        if (value is BoolValue) {
            this.value = (value as BoolValue).value;
        }
    }

    public override void Add(Value value) {
        if (value is BoolValue) {
            this.value = (value as BoolValue).value;
        }
    }

    public override Value Clone(GameObject target) {
        var component = target.AddComponent<BoolValue>();
        component.value = value;

        return component;
    }

    public override string ToString() {
        return value.ToString();
    }
}
