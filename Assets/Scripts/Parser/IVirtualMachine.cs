using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 구문 및 변수를 저장하는 머신
/// </summary>
public interface IVirtualMachine {

    /// <summary>
    /// 주어진 구문을 실행한다
    /// </summary>
    /// <param name="node">파싱할 구문</param>
    /// <returns>성공 여부</returns>
    bool TryExecute(Node node);

    /// <summary>
    /// 해당 이름을 가진 변수를 얻으려 시도한다
    /// </summary>
    /// <param name="name">얻으려는 변수의 이름</param>
    /// <param name="value">얻으려는 변수의 값</param>
    /// <returns>성공 여부</returns>
    bool TryGetVar(string name, ref Value value);

    /// <summary>
    /// 해당 이름을 가진 변수의 값을 설정한다
    /// </summary>
    /// <param name="name">설정하려는 변수의 이름</param>
    /// <param name="value">설정하려는 변수의 값</param>
    /// <returns>성공 여부</returns>
    bool TrySetVar(string name, Value value);

    /// <summary>
    /// 설정된 변수를 정리한다
    /// </summary>
    void CleanUp();
}
