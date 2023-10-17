using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

public class KeyEvent : MonoBehaviour
{
    public KeyCode keyCode;

    public UnityEvent OnKeyInput;
    // Use this for initialization
    void Start()
    {
       this. UpdateAsObservable()
            .Where(_ => Input.GetKey(keyCode))
            .Subscribe(_ => OnKeyInput.Invoke())
            .AddTo(gameObject);
    }

}
