using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour {
    public Canvas rootUI;
    public Canvas mainMenuUI;
    public Canvas settingsUI;
    public Canvas gameUI;
    public Canvas pauseMenuUI;
    public Canvas gameOverUI;

    public PlayerController playerController;

    private Canvas[] mainMenuUIGroup;
    private Canvas[] gameUIGroup;

    private bool isGameRunning = false;
    //-----------------------------------
    void Start () {
        mainMenuUIGroup = new Canvas[] { mainMenuUI, settingsUI };
        gameUIGroup = new Canvas[] { gameUI, pauseMenuUI, gameOverUI };

        LoadMainMenu();
    }
	void Update () {
        if (isGameRunning) {
            Time.timeScale = 1f;
        } else {
            Time.timeScale = 0f;
        }
	}
    //-------------------------
    public void StartNewGame() {
        DisableUI(mainMenuUIGroup);
        DisableUI(gameUIGroup);
        EnableUI(gameUI);

        playerController.SetVisible(false);
        playerController.transform.position = Vector3.zero;
        playerController.SetVisible(true);
        ObstacleSpawner.DestroyAllObstacles();
        ObstacleSpawner.StartSpawning();
        GameManager.ResetScore();
        ObstacleSpawner.SetSpawnInterval(80f);
        PlayerController.SetAngularVelocity(10f);
        PlayerController.SetAngularVelocity(10f);
        PlayerController.SetForwardVelocity(20f);
        ObstacleSpawner.SetHoleMin(0.6f);
        ObstacleSpawner.SetHoleMax(0.8f);
        ObstacleBehaviour.SetRotationMultiplier(100f);
        ObstacleSpawner.Reset();

        isGameRunning = true;
    }
    public void LoadMainMenu() {
        ObstacleSpawner.StopSpawning();
        ObstacleSpawner.DestroyAllObstacles();
        playerController.SetVisible(false);
        playerController.transform.position = Vector3.zero;

        DisableUI(mainMenuUIGroup);
        DisableUI(gameUIGroup);
        EnableUI(mainMenuUI);
    }
    public void LoadSettings() {
        DisableUI(mainMenuUIGroup);
        EnableUI(settingsUI);
    }
    public void ResumeGame() {
        DisableUI(gameUIGroup);
        EnableUI(gameUI);

        isGameRunning = true;
    }
    public void LoadPauseMenu() {
        DisableUI(gameUIGroup);
        EnableUI(pauseMenuUI);

        isGameRunning = false;
    }
    public void LoadGameOver() {
        DisableUI(gameUIGroup);
        EnableUI(gameOverUI);

        isGameRunning = false;
    }
    //----------------------
    public void SetUIEnabled(Canvas canvas, bool enabled) {
        canvas.enabled = enabled;
    }
    public void SetUIEnabled(Canvas[] canvasGroup, bool enabled) {
        foreach (Canvas canvas in canvasGroup) {
            canvas.enabled = enabled;
        }
    }
    //-------------
    public void EnableUI(Canvas canvas) {
        SetUIEnabled(canvas, true);
    }
    public void EnableUI(Canvas[] canvasGroup) {
        SetUIEnabled(canvasGroup, true);
    }
    public void DisableUI(Canvas canvas) {
        SetUIEnabled(canvas, false);
    }
    public void DisableUI(Canvas[] canvasGroup) {
        SetUIEnabled(canvasGroup, false);
    }
}
