using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

public class MouseEvent : MonoBehaviour
{
    private Vector3 current_pos;

    public UnityEvent OnMouseInput;
    // Use this for initialization
    void Start()
    {
        this.UpdateAsObservable()
             .Where(_ => Input.mousePosition != current_pos)
             .Subscribe(_ => DistinctMouse())
             .AddTo(gameObject);
    }
    private void DistinctMouse()
    {
        current_pos = Input.mousePosition;
        OnMouseInput.Invoke();
    }
}