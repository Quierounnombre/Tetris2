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

    public void resume(){
        Time.timeScale=1;
        TextMeshProUGUI[] texts=Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        Button[] buttons=Resources.FindObjectsOfTypeAll<Button>();
        foreach(var i in texts){
            if(i.gameObject.CompareTag("PauseMenu"))
                i.gameObject.SetActive(false);
        }

        foreach(var j in buttons){
            if(j.gameObject.CompareTag("PauseMenu"))
                j.gameObject.SetActive(false);
        }
    }

    public void pause(){
        Time.timeScale=0;
        TextMeshProUGUI[] texts=Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        Button[] buttons=Resources.FindObjectsOfTypeAll<Button>();
        foreach(var i in texts)
        {
            if(i.gameObject.CompareTag("PauseMenu"))
            {
                i.gameObject.SetActive(true);
            }
        }

        foreach(var j in buttons)
        {
            if(j.gameObject.CompareTag("PauseMenu"))
			{
                j.gameObject.SetActive(true);
			}
        }
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
}