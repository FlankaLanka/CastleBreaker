using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1f;
    }

    public void LevelSelectMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel()
    {
        string level = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
        //Int32.TryParse(level, out int res);
        SceneManager.LoadScene("Level" + level); //level is just a number 0,1,2,3
        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void SettingsDisplay()
    {
        EventSystem.current.currentSelectedGameObject.transform.Find("SettingsMenu").gameObject.SetActive(true);
    }

    public void XButtonCloseParent()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
    }

}
