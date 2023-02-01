using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager 	instance = null;
    public static int			Score = 0;
	public int					Win_score;
    public Puntuacion			puntuacion;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ChangeScene(string sc)
    {
		Score = 0;
        if (sc == "")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.instance.Awake();
        }
        else 
        {
            if (sc != "Exit")
            {
                SceneManager.LoadScene(sc);
	        }
            else
            {
                Application.Quit();
            };
        }
    }

    public void resume()
    {
        Time.timeScale = 1;
    }

    public void pause()
    {
        Time.timeScale = 0;
    }

    public void gameover()
    {
        Time.timeScale = 0;
        TextMeshProUGUI[] texts=Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        Button[] buttons=Resources.FindObjectsOfTypeAll<Button>();
        foreach(var i in texts)
        {
            if(i.gameObject.CompareTag("GameOverMenu"))
            {
                i.gameObject.SetActive(true);
            }
        }
        foreach(var j in buttons)
        {
            if(j.gameObject.CompareTag("GameOverMenu"))
			{
                j.gameObject.SetActive(true);
			}
        }
    }

    public void restart_game()
    {
        Score = 0;
        ChangeScene("");
    }

    public void score(bool player)
    {
        if (player)
		{
			puntuacion.move_score(1);
            Score++;
		}
        else
		{
			Score--;
			puntuacion.move_score(-1);
		}
        if (Score >= Win_score)
            Time.timeScale = 0; // GAMEOVER
        else if (Score <= -Win_score)
            Time.timeScale = 0; // GAMEOVER
    }

    private void LateUpdate() {
        debug_keys();
    }
    private void debug_keys()
	{
		if (Input.GetKeyDown(KeyCode.T))
			score(true);
		else if (Input.GetKeyDown(KeyCode.Y))
			score(false);
	}
}