using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {
    private MeshRenderer meshRenderer;
    private bool isValuable = true;
    private float holeCenterRadius = 5f;
    private float rotationVelocity = 1f;
    private bool isDestroyScheduled = false;
    private Color targetColor;

    private bool isColorRefreshed = false;

    private static float rotationMultiplier = 1f;
        
    //--------------------------
    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();

        Color color = meshRenderer.material.color;
        color.a = 0.01f;
        meshRenderer.material.color = color;
        meshRenderer.material.renderQueue = 0;
        targetColor = color;
    }
	void Start () {
        rotationVelocity = Random.Range(0.3f, 1f) * (Random.Range(0,1) * 2 - 1);

    }
    public void Init(float holeCenterRadius) {
        this.holeCenterRadius = holeCenterRadius;
    }
    void Update () {
        transform.Rotate(Vector3.forward, rotationVelocity * rotationMultiplier * Time.deltaTime);

        Color color = meshRenderer.material.color;
        

        if (PlayerController.GetPlayerPositionZ() > transform.position.z + 0.1f || isDestroyScheduled)
        {
            // player have passed through a obstacle hole
            if (isValuable && !isDestroyScheduled) GameManager.AddScore((int)PlayerController.GetForwardVelocity());
            isValuable = false;
            color.a = Mathf.MoveTowards(color.a, 0f, Time.unscaledDeltaTime * 2f);
        } else {
            if(!color.Equals(targetColor))
                color = Color.Lerp(color, targetColor, Time.unscaledDeltaTime * 10f);
        }
        meshRenderer.material.color = color;
        // destroy if 
        if (meshRenderer.material.color.a == 0f && (PlayerController.GetPlayerPositionZ() > transform.position.z || isDestroyScheduled)) {
            Destroy(gameObject);
        }
    }
    public void FancyDestroy() {
        isDestroyScheduled = true;
    }
    public bool IsDestroyed()
    {
        return isDestroyScheduled;
    }
    //-----------------------------------

    public static void SetRotationMultiplier(float multiplier)
    {
        rotationMultiplier = multiplier;
    }
    public static float GetRotationMultiplier()
    {
        return rotationMultiplier;
    }
    //----------------------------------
    public float GetHoleCenterRadius() {
        if (!isColorRefreshed) {
            targetColor = ColorStyleManager.GetRandomObstacleColor();
            isColorRefreshed = true;
        }
        return holeCenterRadius;
    }
    public Color GetNormalColor() {
        return meshRenderer.material.color;
    }
    public Color GetInvertedColor() {
        Color color = meshRenderer.material.color;
        color.r = 1 - color.r;
        color.g = 1 - color.g;
        color.b = 1 - color.b;
        return color;
    }
}
