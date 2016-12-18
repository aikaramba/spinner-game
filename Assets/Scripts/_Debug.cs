using UnityEngine;
using System.Collections;

public class _Debug : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ColorStyleManager.ParseRawHTML(Resources.Load("colorsRaw") as TextAsset);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
