using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [field: SerializeField] public float WorldSpeed { get; private set; }

    private const string MAIN_MENU_SCENE_NAME = "Main Menu";
    private const string GAME_OVER_SCENE_NAME = "Game Over";
    private const float SHOW_GAME_OVER_SCREEN_DELAY = 3f;

    public void Pause()
    {
        if (!UIController.Instance.pausePanel.activeSelf)
        {
            UIController.Instance.pausePanel.SetActive(true);
            PlayerController.Instance.DisablePlayerInput();
            Time.timeScale = 0f;
        }
        else
        {
            UIController.Instance.pausePanel.SetActive(false);
            PlayerController.Instance.EnablePlayerInput();
            Time.timeScale = 1f;
        }
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());
    }

    private IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(SHOW_GAME_OVER_SCREEN_DELAY);
        SceneManager.LoadScene(GAME_OVER_SCENE_NAME);
    }
}