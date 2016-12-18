using UnityEngine;
using System.Collections;

public class RadialTouchController : MonoBehaviour {
    [Range(0.01f,30f)]
    public float sensitivity = 1f;
    [Range(1f,100f)]
    public float maxCap = 10f;

    private Vector2 mousePosition = Vector2.zero;
    private Vector2 lastMousePosition = Vector2.zero;
    private Vector2 screenCenter = Vector2.zero;

    private Vector2 mousePositionDir = Vector2.zero;
    private Vector2 lastMousePositionDir = Vector2.zero;
	//------------------------
	void Start () {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

    }
	void Update () {
        // UpdateBoxTouchInput();
        UpdatePreciseRadialTouchInput();
    }
    //-------------------
    void UpdatePreciseRadialTouchInput()
    {
        float newAngle = 0f;
        
        mousePosition = Input.mousePosition; 
        mousePositionDir = mousePosition - screenCenter;
        if (Input.GetMouseButton(0) && Vector3.Distance(mousePosition, screenCenter) > Screen.width / 12f) {
            newAngle = AbsoluteVec2Angle(Vector2.right, mousePositionDir.normalized);
        }
        if(newAngle != 0f)
        TouchInput.SetAxis("Angle", (Mathf.PI * 2f) * (-newAngle/360f) );
    }
    void UpdateBoxTouchInput()
    {
        float shiftAngle = 0f;
        mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0)) {
            lastMousePosition = mousePosition;
        }
        if (Input.GetMouseButton(0) && mousePosition.y < Screen.height / 3f) {
            shiftAngle = mousePosition.x - lastMousePosition.x;
           // shiftAngle = Mathf.Clamp(shiftAngle, -maxCap, maxCap);
        }
        TouchInput.SetAxis("RadialTouchShift", shiftAngle * sensitivity);

        lastMousePosition = mousePosition;
    }
    void UpdateRadialTouchInput() {
        float shiftAngle = 0f;

        lastMousePositionDir = mousePositionDir;
        mousePosition = Input.mousePosition;
        mousePositionDir = mousePosition - screenCenter;
        if (Input.GetMouseButton(0))
        {
            if (lastMousePositionDir != null)
            {
                shiftAngle = SignedVec2Angle(lastMousePositionDir.normalized, mousePositionDir.normalized);
                shiftAngle = Mathf.Clamp(shiftAngle, -maxCap, maxCap);
            }
        }
        else {

        }
        TouchInput.SetAxis("RadialTouchShift", -shiftAngle);
    }

    public static float SignedVec2Angle(Vector2 fromVector2, Vector2 toVector2) {
        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (cross.z > 0)
            ang = - ang;

        return ang;
    }

    public static float AbsoluteVec2Angle(Vector2 fromVector2, Vector2 toVector2)
    {
        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (cross.z > 0)
            ang = 360-ang;

        return ang;
    }
}
