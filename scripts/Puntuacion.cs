using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntuacion : MonoBehaviour
{
    public void    move_score(int v)
    {
		transform.position = new Vector2(transform.position.x + v, transform.position.y);
    }
}
