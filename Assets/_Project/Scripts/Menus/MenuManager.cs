using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace CodeLabTutorial
{
    public class MenuManager : MonoBehaviour
    {
        private const string LEVEL_1_SCENE_NAME = "Level 1";

        private void Awake()
        {
            Time.timeScale = 1f;
        }

        public void NewGame()
        {
            SceneManager.LoadScene(LEVEL_1_SCENE_NAME);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}