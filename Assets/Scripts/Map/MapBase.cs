using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBase : MonoBehaviour {
    public GameState MainView {
        get { return (views.Count > 0 ? views[0] : null); }
    }

    [SerializeField]
    protected GameObject blocks;

    [SerializeField]
    protected GameObject enviroments;

    [SerializeField]
    protected List<GameState> views;
}
