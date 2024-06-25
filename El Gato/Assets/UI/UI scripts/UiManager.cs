using JetBrains.Annotations;
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
    public GameObject levelSelect;
    public GameObject shop;
    public GameObject upgrades;
   
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

    [System.Obsolete]
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
        credits.SetActive(true);
    }

    public void HideCredits()
    { 
        mainMenu.SetActive(true);
        credits.SetActive(false);
    }

    public void ShowLevleSelect()
    {
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);

    }

    public void HideLevelSelect()
    {
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
    }

    
    public void GoToLevelTwo()
    {
        Application.LoadLevel(2);
    }

    public void ShowUpgrades()
    {
        upgrades.SetActive(true);
        shop.SetActive(false);
    }

    public void HideUpgrades()
    {
        upgrades.SetActive(false);
        shop.SetActive(true);
    }
}

