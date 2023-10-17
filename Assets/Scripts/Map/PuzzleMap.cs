using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMap : MapBase {

    public void SpawnBlock(BlockOnPuzzleMap block, Vector3 position) {
        var obj = Instantiate(block, position, Quaternion.identity);
        obj.transform.parent = blocks.transform;
    }
}
