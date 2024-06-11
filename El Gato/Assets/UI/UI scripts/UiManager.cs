using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject lossScreen;
    public GameObject winScreen;
    public GameObject settings;
    public GameObject mainMenu;
    public GameObject credits;

    [System.Obsolete]
    public void ShowGameOverScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        lossScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowWinScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        Application.LoadLevel(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToGame()
    {
        Time.timeScale = 1;
        Application.LoadLevel(1);
    }

    public void ShowSettings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void HideSettings()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(false);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }
}
