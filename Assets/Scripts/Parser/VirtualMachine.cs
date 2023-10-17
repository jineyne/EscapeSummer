using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMachine : MonoSingleton<VirtualMachine>, IVirtualMachine {
    public List<Node> InitNodes;

    [SerializeField]
    private SerializableDictionary<string, Value> varData = new SerializableDictionary<string, Value>();

    protected void Awake() {
        base.Awake();

        CleanUp();
    }

    public bool TryExecute(Node node) {
        return node.Visit(this) != null;
    }

    public bool TryGetVar(string name, ref Value value) {
        if (!varData.ContainsKey(name)) {
            return false;
        }

        value = varData[name];
        return true;
    }

    public bool TrySetVar(string name, Value value) {
        if (varData.ContainsKey(name) && varData[name].GetType() != value.GetType()) {
            return false;
        }

        varData[name] = value;
        return true;
    }

    public void CleanUp() {
        foreach (var pair in varData) {
            Destroy(pair.Value.gameObject);
            Destroy(pair.Value);
        }

        varData = new SerializableDictionary<string, Value>();
        foreach (var node in InitNodes) {
            TryExecute(node);
        }
    }
}
