using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameManager 	GameManager;
    public Vector2Int			pos;

	private void Awake() 
	{
		pos = new Vector2Int(5, 10);
	}

    void Update()
    {
        if(Input.GetButton("Cancel"))
        {
            GameManager.instance.pause();
        }
    }
}
