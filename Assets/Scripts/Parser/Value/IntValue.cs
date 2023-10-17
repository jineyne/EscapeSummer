using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 정수형을 나타내는 값
/// </summary>
public class IntValue : Value {
    public int Value {
        get { return value; }
    }

    [SerializeField]
    private int value;

    public override Value Visit(IVirtualMachine machine) {
        return this;
    }

    public override void Set(Value value) {
        if (value is IntValue) {
            this.value = (value as IntValue).value;
        }
    }

    public override void Add(Value value) {
        if (value is IntValue) {
            this.value += (value as IntValue).value;
        }
    }

    public override Value Clone(GameObject target) {
        var component = target.AddComponent<IntValue>();
        component.value = value;

        return component;
    }

    public override string ToString() {
        return value.ToString();
    }
}
