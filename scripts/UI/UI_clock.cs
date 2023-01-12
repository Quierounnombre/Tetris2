using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_clock : MonoBehaviour
{
	public TextMeshProUGUI 	txt;
	public float			delta_time;
	private float			start_time;

	private void Start()
	{
		start_time = Time.time;
	}
	private void	Update()
	{
		if (Time.time > (delta_time + start_time))
		{
			delta_time += 1;
			txt.text = format_time();
		}
	}

	private string	format_time()
	{
		float	minutes;
		float	seconds;
		string	s_time;

		minutes = Mathf.Floor(delta_time / 60);
		seconds = Mathf.RoundToInt(delta_time - minutes * 60);
		s_time = string.Format("{0:0}:{1:00}", minutes, seconds);
		return (s_time);
	}
}