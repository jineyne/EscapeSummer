using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Parchment : MonoBehaviour {
    [SerializeField]
    private Collider2D _collider;

    [SerializeField]
    private ParchmentUI targetUI;

    [SerializeField]
    private Vector3 customPosition;

    [TextArea]
    public string text;

    private void Start() {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        targetUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && targetUI != null) {
            targetUI.gameObject.SetActive(true);
            targetUI.text.text = this.text;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && targetUI != null) {
            targetUI.gameObject.SetActive(false);
        }
    }
}
