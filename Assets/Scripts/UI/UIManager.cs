using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIStageType {
    Puzzle,
    Interaction
}

public enum EndingType {
    ItIsSummer,
    ItWasSummer,
}

public class UIManager : MonoSingleton<UIManager> {
    public UIStageType Stage { get { return stage; }}

    public EndingType EndingType { get { return endingType; } }

    public Vector3 Offset { 
        get { return offset; } 
        set { offset = value; } 
    }

    public UIBase ActiveUI { get; set; }

    [SerializeField]
    private UIStageType stage;

    [SerializeField]
    private EndingType endingType;

    [SerializeField]
    private Vector3 offset;

    public void SetStage(UIStageType type) {
        stage = type;
    }

    public void SetEnding(EndingType type) {
        endingType = type;
    }

}
