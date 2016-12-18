using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreValueDisplay : MonoBehaviour {
    Text label;
    // Use this for initialization
    void Awake() {
        label = GetComponent<Text>();
    }
	void Start () {
	    
	}
	
	// Update is called once per frame
	void LateUpdate () {
        label.text = GameManager.GetScore().ToString();

    }
}
