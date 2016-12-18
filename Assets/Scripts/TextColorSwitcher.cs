using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextColorSwitcher : MonoBehaviour {
    private Text label;

    private static Color targetColor = Color.white;
	//-------------------------------------
    void Awake() {
        label = GetComponent<Text>();
    }
	void Start () {
        
    }
	void Update () {
	}
    void FixedUpdate() {
        if (label && !label.color.Equals(targetColor))
            label.color = Color.Lerp(label.color, targetColor, Time.deltaTime * 20.0f);
    }
    //-------------------------------
    public static void ChangeColor(Color color) {
        targetColor = color;
        
    }
}
