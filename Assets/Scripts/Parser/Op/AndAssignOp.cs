using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 좌우 구문을 가지고 수학적 계산하는 연산자 노드
/// </summary>
public class AndAssignOp : Op {
    public override Value Visit(IVirtualMachine machine) {
        var rightResult = RightNode.Visit(machine);
        var leftResult = LeftNode.Visit(machine);

        if (rightResult == null) {
            return null;
        }

        if (!(rightResult is BoolValue)) {
            return null;
        }

        if (LeftNode is Var) {
            (LeftNode as Var).Add(machine, rightResult);

            return rightResult;
        } else if (leftResult != null && leftResult is VarValue) {
            (leftResult as VarValue).Add(rightResult);

            return leftResult;
        }

        return null;
    }
}
