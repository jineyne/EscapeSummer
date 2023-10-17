using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float Weight;
    public void CanInteract()
    {

    }
    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y / -0.5f);
    }
}
