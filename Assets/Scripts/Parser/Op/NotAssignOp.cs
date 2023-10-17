using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 좌우 구문을 가지고 수학적 계산하는 연산자 노드
/// </summary>
[RequireComponent(typeof(BoolValue))]
public class NotAssignOp : Op {
    [SerializeField]
    private BoolValue internalBoolValue;

    public void Start() {
        if (internalBoolValue == null) {
            internalBoolValue = GetComponent<BoolValue>();
            if (internalBoolValue == null) {
                internalBoolValue = gameObject.AddComponent<BoolValue>();
            }
        }
    }
    public override Value Visit(IVirtualMachine machine) {
        var rightResult = RightNode.Visit(machine);
        var leftResult = LeftNode.Visit(machine);

        if (rightResult == null) {
            return null;
        }

        if (!(rightResult is BoolValue)) {
            return null;
        }

        var boolValue = rightResult as BoolValue;
        internalBoolValue.Value = !boolValue.Value;

        if (LeftNode is Var) {
            (LeftNode as Var).Add(machine, internalBoolValue);

            return rightResult;
        } else if (leftResult != null && leftResult is VarValue) {
            (leftResult as VarValue).Add(boolValue);

            return leftResult;
        }

        return null;
    }
}
