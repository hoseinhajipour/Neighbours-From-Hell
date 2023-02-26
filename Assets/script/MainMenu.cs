using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void loadlevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
