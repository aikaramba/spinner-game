using UnityEngine;
using System.Collections;

public class PlayerTrailFollow : MonoBehaviour {
    public Transform player;
    //----------------
	void Start () {
	    
	}
	
	void Update () {
        if (Vector3.Distance(transform.position, player.position) > 10.0f) transform.position = player.position;
         transform.position = Vector3.Slerp(transform.position, player.position, Time.deltaTime * Vector3.Distance(transform.position, player.position) * 10f);
	}
}
