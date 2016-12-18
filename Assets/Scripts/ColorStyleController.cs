using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorStyleController : MonoBehaviour {

    //---------------------
    void Awake() {
        foreach (Text txtT in Object.FindObjectsOfType<Text>()) {
            txtT.gameObject.AddComponent<TextColorSwitcher>();
        }
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //------------------------
    public static void SetNewTextColor(Color color) {
        TextColorSwitcher.ChangeColor(color);
    }
}
