using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.SetStage(UIStageType.Interaction);
    }
}
