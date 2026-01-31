// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class MainMenu : MonoBehaviour
// {
//     private string newGameScene = "Dock Thing";
//
//     public TMP_Text highScoreUI;
//
//     public AudioClip bg_music;
//     public AudioSource main_channel;
//     
//     void Start()
//     {
//         main_channel.PlayOneShot(bg_music);
//         
//         //SEt the high score text
//
//         int highScore = SaveLoadManager.Instance.LoadHighScore();
//         highScoreUI.text = $"Top Wave Zurvival: {highScore}";
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//     }
//
//     public void StartNewGame()
//     {
//         main_channel.Stop();
//         SceneManager.LoadScene(newGameScene);
//     }
//
//     public void ExitApplication()
//     {
// #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
// #else
//     Application.Quit();
// #endif
//     }
//    
// }

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string newGameScene = "Map_v1";
    [SerializeField] private TMP_Text highScoreUI;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioSource mainChannel;

    private void Start()
    {
        PlayBackgroundMusic();
        DisplayHighScore();
        ConfigureCursor();
    }

    private void PlayBackgroundMusic()
    {
        mainChannel.PlayOneShot(bgMusic);
    }

    private void DisplayHighScore()
    {
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"top wave zurvived {highScore}";
    }

    private void ConfigureCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartNewGame()
    {
        mainChannel.Stop();
        SceneManager.LoadScene(newGameScene);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
