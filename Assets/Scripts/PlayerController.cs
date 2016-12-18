using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public float angularVelocityMax = 10f;
    public float forwardVelocityMax = 10f;
    public float flyingRadius = 50f;
    private static float targetflyingRadius = 50f;
    private static float angularVelocity = 0f;
    private static float forwardVelocity = 0f;
    public float angle = 0f;

    private static float playerPositionZ = 0f;

    public MeshRenderer meshRenderer;
    public TrailRenderer trailRenderer;
    public GameStateController gameStateController;

    private static Color targetColor;


    private bool isVisible = true;
    // Use this for initialization
    void Awake() {
       // meshRenderers = GetComponentsInChildren<MeshRenderer>();
       // trailRenderers = GetComponentsInChildren<TrailRenderer>();
    }
    void Start() {
      //  angularVelocity = angularVelocityMax / (2f * Mathf.PI);
      //  forwardVelocity = forwardVelocityMax;
        targetflyingRadius = flyingRadius;
        playerPositionZ = 0f;

        trailRenderer.material.renderQueue = -1;
    }

    // Update is called once per frame
    void Update() {
        if (!isVisible) return;
        UpdateRadius();
        UpdateControls();
        UpdateMovement();
        UpdateColor();
        CheckForCollision();
    }
    void LateUpdate() {

    }
    //---------------------------
    void UpdateRadius() {
        RaycastHit hit;
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Obstacles");
        if (Physics.Raycast(new Vector3(0f, 0f, playerPositionZ), Vector3.forward, out hit, 100.0f, layerMask)) {
            ObstacleBehaviour hitObstacleBehaviour = hit.transform.GetComponent<ObstacleBehaviour>();
            if (hitObstacleBehaviour) {
                SetNewFlyingRadius(hitObstacleBehaviour.GetHoleCenterRadius());
                CameraController.SetNewBackgroundColor(hitObstacleBehaviour.GetInvertedColor());
            }
        }
    }
    void UpdateControls() {
        /*
        if (Input.GetMouseButtonDown(0)) {
            angularVelocity = -angularVelocity;
        }
        */
    }
    void UpdateMovement() {
        if(Time.timeScale != 0f)
        angle = TouchInput.GetAxis("Angle");

        //angle += angularVelocity * TouchInput.GetAxis("RadialTouchShift") * Time.deltaTime;
        //if (angle > 2f * Mathf.PI) angle = 0f;
        //if (angle < 0f) angle = 2f * Mathf.PI;
        flyingRadius = Mathf.MoveTowards(flyingRadius, targetflyingRadius, Time.deltaTime * angularVelocityMax);
        Vector3 newPosition = new Vector3(flyingRadius * Mathf.Cos(angle), flyingRadius * Mathf.Sin(angle), transform.position.z);
        transform.position = newPosition;

        Vector3 newForwardPos = transform.position + (Vector3.forward * forwardVelocity * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, newForwardPos, 50f);

        transform.LookAt(new Vector3(0f, 0f, transform.position.z));
        playerPositionZ = transform.position.z;
    }
    void UpdateColor() {
        trailRenderer.material.SetColor("_TintColor", Color.Lerp(trailRenderer.material.GetColor("_TintColor"), targetColor, Time.deltaTime * 5.0f));
        meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, targetColor, Time.deltaTime); 
       // trailRenderer.material.color = Color.Lerp(trailRenderer.material.color, targetColor, Time.deltaTime);
    }
    //-------------------------
    void CheckForCollision() {
        RaycastHit hit;
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Obstacles");
        if (Physics.Raycast(transform.position - (Vector3.forward * 1f), Vector3.forward, out hit, 2f, layerMask)) {
            if (hit.transform.name.Contains("Obstacle"))
            {
                if (!hit.transform.GetComponent<ObstacleBehaviour>().IsDestroyed())
                {
                    gameStateController.LoadGameOver();
                }
            }
        }
        
    }
    //-----------------------------

    //--------------------------
    public void SetVisible(bool isVisible) {
        if (this.isVisible == isVisible) return;

        
        meshRenderer.enabled = isVisible;
        //trailRenderer.enabled = isVisible;
        trailRenderer.gameObject.SetActive(isVisible);
        trailRenderer.time = isVisible ? 1f : 0f;
        

        this.isVisible = isVisible;
    }
    //-------
    public static void ChangeColor(Color color) {
        targetColor = color;
    }


    public static void SetNewFlyingRadius(float newFlyingRadius) {
        targetflyingRadius = newFlyingRadius;
    }
    public static float GetPlayerPositionZ() {
        return playerPositionZ;
    }
    public static float GetForwardVelocity() {
        return forwardVelocity;
    }
    public static void SetForwardVelocity(float forwardVelocity) {
        PlayerController.forwardVelocity = forwardVelocity;
    }
    public static float GetAngularVelocity()
    {
        return angularVelocity * (2f*Mathf.PI);
    }
    public static void SetAngularVelocity(float angularVelocity)
    {
        PlayerController.angularVelocity = angularVelocity / (2f * Mathf.PI);
    }
}
