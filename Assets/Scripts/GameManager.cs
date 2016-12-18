using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class GameManager {
    private static int score;
    private static float totalMult = 1f;
    public static void AddScore(int amount) {
        score += amount;
        float playerForwardVelocity = PlayerController.GetForwardVelocity();
        if (playerForwardVelocity < 70f)
            PlayerController.SetForwardVelocity(playerForwardVelocity*1.25f* totalMult);

        PlayerController.SetAngularVelocity(PlayerController.GetAngularVelocity()*1.01f* totalMult);

        float spawnInterval = ObstacleSpawner.GetSpawnInterval();
        if (spawnInterval > 10f)
            ObstacleSpawner.SetSpawnInterval(spawnInterval - 0.1f* totalMult);

        float holeRatioMin = ObstacleSpawner.GetHoleMin();
        float holeRatioMax = ObstacleSpawner.GetHoleMax();
        if (holeRatioMin > 0.2f) {
            ObstacleSpawner.SetHoleMin(holeRatioMin - 0.05f);
            ObstacleSpawner.SetHoleMax(holeRatioMax - 0.05f);
        }
        float rotationMultiplier = ObstacleBehaviour.GetRotationMultiplier();
        if (rotationMultiplier < 50.0f) {
            ObstacleBehaviour.SetRotationMultiplier(rotationMultiplier + 0.05f * totalMult);
        }
    }
    public static void ResetScore() {
        score = 0;
    }
    public static int GetScore() {
        return score;
    }
}
