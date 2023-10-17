using UnityEngine;
using System.Collections;


// �̱��� : ������ �ϳ��� ��ü�θ� ���ɰ�� ��Ŭ������ ��ӹ޾Ƽ� ���
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                try
                {
                    _instance = (T)FindObjectOfType<T>();
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.StackTrace);
                    return null;
                }
            }
            return _instance;
        }
    }
}