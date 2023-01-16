using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_color : MonoBehaviour
{
    public Color			P1_color;
    public Color			P2_color;
	public SpriteRenderer	rd;

    public void color_swap(bool is_P1)
    {
		if (is_P1)
			rd.color = P1_color;
		else
			rd.color = P2_color;
    }
}
