using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour {
    public Transform playerTr;
    public float distanceFromPlayer = 100f;
    public float spawnRate;
    
    
    public int numOfIterations = 40;
    public float innerObstacleRadiusMin;
    public float innerObstacleRadiusMax;
    public float holeDistanceMin;
    public float holeDistanceMax;
    public float outerObstacleRadiusMin;
    public float outerObstacleRadiusMax;

    private static float holeRatioMin = 0.6f;
    private static float holeRatioMax = 0.8f;

    public Material obstacleMaterial;

    private static float spawnInterval = 50f;
    private static Vector3 lastSpawnPosition = Vector3.zero;
    private float spawnTimer = 0f;
    private static bool isSpawning = false;
    
    private Dictionary<float, List<MeshDistancePair>> generatedMeshDB = new Dictionary<float, List<MeshDistancePair>>();
    //-----------------
    void Awake () {
        GenerateMeshDB();
    }
	void Start () {
       
    }
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, playerTr.position.z + distanceFromPlayer);

        if (isSpawning)
        {
            if (Vector3.Distance(transform.position, lastSpawnPosition) > spawnInterval)
            {
                lastSpawnPosition = transform.position;

                MeshDistancePair mdpT = GetMeshDistancePair(Random.Range(GetHoleMin(), GetHoleMax()));
                Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0f);
                SpawnNewObstacle(
                    mdpT.mesh,
                    mdpT.distance,
                    obstacleMaterial,
                    color);
            }
        }
	}
    //--------
    void GenerateMeshDB() {
        for (float fT = 0.1f; fT <= 0.9f; fT += 0.1f) {
            List<MeshDistancePair> newDBList = new List<MeshDistancePair>();
            for (int i = 0; i < 100; i++){
                float innerObstacleRadius = Random.Range(innerObstacleRadiusMin, innerObstacleRadiusMax);
                float holeDistance = innerObstacleRadius + Random.Range(holeDistanceMin, holeDistanceMax);
                float outerObstacleRadius = holeDistance + Random.Range(outerObstacleRadiusMin, outerObstacleRadiusMax);
                float holeRatio = fT;
                
                Mesh newMesh = GenerateNewMesh(numOfIterations, innerObstacleRadius, holeDistance, outerObstacleRadius, holeRatio);
                float passDistance = (innerObstacleRadius + holeDistance) / 2f;
                newDBList.Add(new MeshDistancePair(newMesh, passDistance));
            }
            generatedMeshDB.Add(fT, newDBList);
        }
    }
    MeshDistancePair GetMeshDistancePair(float distance) {
        foreach (float keyT in generatedMeshDB.Keys) {
            if (Mathf.Abs(keyT - distance) < 0.1f) {
                return generatedMeshDB[keyT][Random.Range(0, generatedMeshDB[keyT].Count)];
            }
        }
        return null;
    }
    //--------------------------
    public static void SetHoleMin(float ratio) {
        holeRatioMin = ratio;
    }
    public static float GetHoleMin() {
        return holeRatioMin;
    }
    public static void SetHoleMax(float ratio) {
        holeRatioMax = ratio;
    }
    public static float GetHoleMax() {
        return holeRatioMax;
    }

    public static void SetSpawnInterval(float interval) {
        spawnInterval = interval;
    }
    public static float GetSpawnInterval() {
        return spawnInterval;
    }
    public static void StopSpawning() {
        isSpawning = false;
    }
    public static void StartSpawning(){
        isSpawning = true;
    }
    public static void DestroyAllObstacles() {
        foreach (ObstacleBehaviour obT in Object.FindObjectsOfType<ObstacleBehaviour>()) {
            obT.FancyDestroy();
        }
    }
    public static void Reset() {
        lastSpawnPosition = Vector3.zero;
    }
    protected void SpawnNewObstacle(
        Mesh obstacleMesh,
        float passDistance,
        Material obstacleMaterial,
        Color materialColor)
    {
        // instantiating the mesh object and parenting it to the unit
        GameObject obstacleGO = new GameObject("Obstacle");
        //obstacleGO.transform.SetParent(transform);
        obstacleGO.transform.position = transform.position;
        obstacleGO.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 359f)));

        MeshFilter obstacleMeshFilter = obstacleGO.AddComponent<MeshFilter>();
        MeshRenderer obstacleMeshRenderer = obstacleGO.AddComponent<MeshRenderer>();
        MeshCollider obstacleMeshCollider = obstacleGO.AddComponent<MeshCollider>();
        ObstacleBehaviour obstacleBehaviour = obstacleGO.AddComponent<ObstacleBehaviour>();
        
        obstacleGO.layer = LayerMask.NameToLayer("Obstacles");
        obstacleBehaviour.Init(passDistance);



        //----------------
        // applying mesh and material
        obstacleMeshFilter.mesh = obstacleMesh;
       // obstacleMeshFilter.mesh.RecalculateNormals();
        obstacleMeshRenderer.material = obstacleMaterial;
        obstacleMeshRenderer.material.color = materialColor;

        obstacleMeshCollider.sharedMesh = obstacleMeshFilter.mesh;

        
    }

    private Mesh GenerateNewMesh(
        int numOfIterations,
        float innerObstacleRadius,
        float holeDistance,
        float outerObstacleRadius,
        float holeRatio) {

        // figuring out future points of the circle
        Vector3[] innerCirclePoints = new Vector3[numOfIterations];
        Vector3[] holeCirclePoints = new Vector3[numOfIterations];
        Vector3[] outerCirclePoints = new Vector3[numOfIterations];
        float stepAngle = 2f * Mathf.PI / numOfIterations;
        float angle = 0.0f;
        for (int n = 0; n < numOfIterations; n++)
        {
            float viewHeight = 0f;
            Vector3 innerPoint = new Vector3(innerObstacleRadius * Mathf.Cos(angle), innerObstacleRadius * Mathf.Sin(angle), viewHeight);
            Vector3 holeCirclePoint = innerPoint.normalized * holeDistance;
            Vector3 outerPoint = innerPoint.normalized * outerObstacleRadius;
            innerCirclePoints[n] = innerPoint;
            holeCirclePoints[n] = holeCirclePoint;
            outerCirclePoints[n] = outerPoint;
            angle += stepAngle;
        }
        
        //----------------------
        // generating mesh itself
        //-------------
        // inner circle
        Mesh mesh = new Mesh();
        int numOfPoints = innerCirclePoints.Length;

        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();

        // make first triangle
        vertexList.Add(Vector3.zero);  // 1. Circle center.
        vertexList.Add(innerCirclePoints[0]);  // 2. First vertex on circle outline
        vertexList.Add(innerCirclePoints[1]);     // 3. First vertex on circle outline rotated by angle

        // add triangle indices.
        triangleList.Add(0);
        triangleList.Add(2);
        triangleList.Add(1);

        for (int i = 2; i < numOfPoints; i++)
        {
            triangleList.Add(0);                      // Index of circle center.
            triangleList.Add(vertexList.Count);     // main triangle
            triangleList.Add(vertexList.Count - 1);
            vertexList.Add(innerCirclePoints[i]);
            //    uvList.Add(new Vector2(1f, 1f));
        }
        triangleList.Add(0);
        triangleList.Add(1);
        triangleList.Add(vertexList.Count - 1);

        //-------------
        //hole circle

        int holeBreakPoint = Mathf.CeilToInt((1 - holeRatio) * (numOfPoints - 2));
        int vertexCount = vertexList.Count - 1;

        vertexList.Add(holeCirclePoints[0]);
        vertexList.Add(innerCirclePoints[0]);
        for (int i = 1; i < numOfPoints; i++)
        {
            vertexList.Add(holeCirclePoints[i]);
            vertexList.Add(innerCirclePoints[i]);
            vertexCount = vertexList.Count - 1;
            triangleList.Add(vertexCount);                      // Index of circle center.
            triangleList.Add(vertexCount - 1);
            triangleList.Add(vertexCount - 3);
            triangleList.Add(vertexCount);                      // Index of circle center.
            triangleList.Add(vertexCount - 3);
            triangleList.Add(vertexCount - 2);
            if (i > holeBreakPoint) break;
        }

        //---------------
        // outer circle
        vertexList.Add(outerCirclePoints[0]);
        vertexList.Add(holeCirclePoints[0]);
        for (int i = 1; i < numOfPoints; i++)
        {
            vertexList.Add(outerCirclePoints[i]);
            vertexList.Add(holeCirclePoints[i]);

            vertexCount = vertexList.Count - 1;
            triangleList.Add(vertexCount);                      // Index of circle center.
            triangleList.Add(vertexCount - 1);
            triangleList.Add(vertexCount - 3);
            triangleList.Add(vertexCount);                      // Index of circle center.
            triangleList.Add(vertexCount - 3);
            triangleList.Add(vertexCount - 2);
        }
        vertexList.Add(outerCirclePoints[0]);
        vertexList.Add(holeCirclePoints[0]);

        vertexCount = vertexList.Count - 1;
        triangleList.Add(vertexCount);                      // Index of circle center.
        triangleList.Add(vertexCount - 1);
        triangleList.Add(vertexCount - 3);
        triangleList.Add(vertexCount);                      // Index of circle center.
        triangleList.Add(vertexCount - 3);
        triangleList.Add(vertexCount - 2);


        mesh.SetVertices(vertexList);
        mesh.SetIndices(triangleList.ToArray(), MeshTopology.Triangles, 0);
        mesh.RecalculateNormals();

        // clearing leftovers
        triangleList.Clear();
        triangleList = null;
        vertexList.Clear();
        vertexList = null;

        return mesh;
    }
}

public class MeshDistancePair {
public Mesh mesh;
public float distance;
public MeshDistancePair(Mesh mesh, float distance) {
        this.distance = distance;
        this.mesh = mesh;
}
}