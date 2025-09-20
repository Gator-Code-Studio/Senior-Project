using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
            Time.timeScale = 0; // time stop
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1; // time resume
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
