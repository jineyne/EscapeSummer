using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오른쪽 노드를 왼쪽 변수에 넣는 연산자 노드
/// </summary>
public class AssignOp : Op {
    public override Value Visit(IVirtualMachine machine) {
        var rightResult = RightNode.Visit(machine);
        var leftResult = LeftNode.Visit(machine);

        if (rightResult == null) {
            return null;
        }

        if (LeftNode is Var) {
            (LeftNode as Var).Set(machine, rightResult);
            return rightResult;
        } else if (leftResult != null && leftResult is VarValue) {
            (leftResult as VarValue).Set(rightResult);
            return leftResult;
        }

        return null;
    }
}
