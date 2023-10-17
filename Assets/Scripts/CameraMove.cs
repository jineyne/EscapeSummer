using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public GameState current_view;
    public float CameraMoveTime = 1f;
    public void change_view()
    {
        current_view = GameStateManager.instance.currentState;
        Vector3 pos = current_view.transform.position;
        pos.z = -10;
        transform.DOMove(pos, CameraMoveTime)
            .SetEase(Ease.OutCirc);
    }
    void Awake()
    {
        this.UpdateAsObservable()
            .Where(_ => (current_view==null || current_view != GameStateManager.instance.currentState))
            .Subscribe(_ => change_view())
            .AddTo(gameObject);
    }
}
