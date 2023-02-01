using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_button : MonoBehaviour
{
    public GameManager  gm;
    public bool         is_pause;
    public Color        pulsed_color;
    public Color        clean_color;
    public Pause_button other_button;

    private void OnMouseDown()
    {
        if (is_pause && Time.timeScale == 1)
        {
            gameObject.SpriteRenderer.color = pulsed_color;
            other_button.SpriteRenderer.color = clean_color;
            gm.pause();
        }
        else if (!is_pause && Time.timeScale == 0)
        {
            gameObject.SpriteRenderer.color = pulsed_color;
            other_button.SpriteRenderer.color = clean_color;
            gm.resume();
        }
    }
}
