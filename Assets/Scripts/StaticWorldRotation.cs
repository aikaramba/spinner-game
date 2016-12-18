using UnityEngine;
using System.Collections;

public class StaticWorldRotation : MonoBehaviour {
    private Vector3 initWorldEuler = Vector3.zero;
	// Use this for initialization
	void Start () {
        initWorldEuler = transform.eulerAngles;

    }
	
	// Update is called once per frame
	void Update () {
        initWorldEuler = transform.eulerAngles = initWorldEuler;

    }
}
