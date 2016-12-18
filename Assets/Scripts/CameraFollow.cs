using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    float zOffset = 0f;
	// Use this for initialization
	void Start () {
        zOffset = target.position.z + transform.position.z;

    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z + zOffset);
	}
}
