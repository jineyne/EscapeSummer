using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 좌우 구문을 가지고 계산하는 연산자 노드
/// </summary>
public abstract class Op : Node {
    /// <summary>
    /// 오른쪽에 존재하는 노드
    /// </summary>
    public Node LeftNode;

    /// <summary>
    /// 왼쪽에 존재하는 노드
    /// </summary>
    [SerializeField]
    public Node RightNode;
}
