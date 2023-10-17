using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlockOnInteractionMap : MonoBehaviour {
    public Collider2D _collider;

    public BlockOnPuzzleMap puzzleMapPrefab;

    public Vector3 customPosition;

    private void Start() {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && puzzleMapPrefab != null) {
            (UIManager.Instance.ActiveUI as StageUI).PuzzleMap.SpawnBlock(puzzleMapPrefab, customPosition);
            Destroy(gameObject);
        }
    }
}
