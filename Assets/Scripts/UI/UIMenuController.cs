using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
    //элементы меню
    [SerializeField] private GameObject GameOverPanel;

    private static bool gameIsOver = false;

    private void Start()
    {
        PlayerAnchor.OnGameOver += StopGame;
    }

    private void OnDestroy()
    {
        PlayerAnchor.OnGameOver -= StopGame;
    }

    public void StartGame()
    {
        if (gameIsOver)
        {
            gameIsOver = false;
            GameOverPanel.SetActive(false);

            Time.timeScale = 1f;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    public void StopGame()
    {
        if (!gameIsOver)
        {
            gameIsOver = true;
            GameOverPanel.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}
