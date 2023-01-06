using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Puntuacion : MonoBehaviour
{
	private int	score;
	public TextMeshProUGUI txt;
	private void Start()
	{
		transform.position = new Vector3(0f, 0f, 0f);
		score = 0;
	}
    public void    move_score(int v)
    {
		transform.position = new Vector3((transform.position.x + v), 0, 0);
		score += v;
		txt.text = score.ToString();
    }
}
