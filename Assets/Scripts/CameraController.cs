using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    new protected Camera camera;
    protected static Color targetBackgroundColor = Color.white;
    //---------------------------
    void Awake() {
        camera = GetComponent<Camera>();
    }
	void Start () {

    }
	void Update () {
        
    }
    void LateUpdate() {

    }
    void FixedUpdate() {
        if(!camera.backgroundColor.Equals(targetBackgroundColor))
            camera.backgroundColor = Color.Lerp(camera.backgroundColor, targetBackgroundColor, Time.deltaTime * 10.0f);
    }
    //---------------------
    public static void SetNewBackgroundColor(Color color) {
        targetBackgroundColor = color;
    }
}
