using UnityEngine;
using System.Collections;

public class CacheCleanup : MonoBehaviour {
    public float interval = 15.0f;
    // Use this for initialization
    void Start () {
        QualitySettings.SetQualityLevel(3);
        InvokeRepeating("ClearAssetCache", 1.0f, interval);
    }

    // Update is called once per frame
    void Update () {
	
	}
    // removing unused assets to free RAM
    private void ClearAssetCache()
    {
       Resources.UnloadUnusedAssets();
    }
}
