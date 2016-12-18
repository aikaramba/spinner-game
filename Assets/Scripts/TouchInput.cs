using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TouchInput {
    static private Dictionary<string, float> inputs = new Dictionary<string, float>();
    static public float GetAxis(string axisName) {
        if (!inputs.ContainsKey(axisName)) {
            inputs.Add(axisName, 0);
        }
        return inputs[axisName];
    }
    static public void SetAxis(string axisName, float value) {
        if (!inputs.ContainsKey(axisName)) {
            inputs.Add(axisName, 0);
        }
        inputs[axisName] = value;
    }
}
