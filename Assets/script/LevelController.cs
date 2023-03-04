using System;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int SabotageCount;
    public int SabotageMax;


    public GameObject WinGamePanel;

    public TextMeshProUGUI SabotageText;

    public void Start()
    {
        SabotageText.text = SabotageCount + " / " + SabotageMax;
    }

    public void CheckWinGame()
    {
        if (SabotageCount >= SabotageMax)
        {
            WinGamePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Increse()
    {
        SabotageCount++;
        SabotageText.text = SabotageCount + "/" + SabotageMax;
        CheckWinGame();
    }
}