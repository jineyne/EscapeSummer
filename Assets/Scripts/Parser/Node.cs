using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 문법의 기본이 되는 노드
/// </summary>
public abstract class Node : MonoBehaviour {
    /// <summary>
    /// 해당 노드를 방문했을 때 할 행동
    /// </summary>
    /// <param name="machine">방문한 머신</param>
    /// <returns>방문 한 결과</returns>
    public abstract Value Visit(IVirtualMachine machine);
}
