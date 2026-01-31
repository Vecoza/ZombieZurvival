using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameUI;
    [SerializeField] private GameObject DefaultGameUI;

    public static bool activePause = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseGameUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        DefaultGameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseGameUI.SetActive(true);
        Time.timeScale = 0f;
        activePause = true;
    }

    public void ResumeGame()
    {       
        DefaultGameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseGameUI.SetActive(false);
        Time.timeScale = 1f;
        activePause = false;
    }

    public void GoToGameMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        activePause = false;
        DefaultGameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
