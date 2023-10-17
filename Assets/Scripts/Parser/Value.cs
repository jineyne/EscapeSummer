using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Value : Node {
    public abstract void Set(Value value);

    public abstract void Add(Value value);

    public abstract Value Clone(GameObject target);
}
