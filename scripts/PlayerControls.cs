using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameManager 	GameManager;
	public board		board;
	public piece		piece;
    public Vector2Int	pos;
    public float        timedelay;
    private float       deltatime;
    

    void Update()
    {
		if (Time.time >= deltatime)
		{
			if (board.IsValid(new Vector2Int(0, -1), piece))
				Drop();
			else
				new_piece();
		}
    }

	public void Awake()
	{
		new_piece();
	}

	public void new_piece()
	{
		piece = board.gen.generate();
		deltatime = Time.time + timedelay;
		board.spawn_piece(piece);
	}

	public void Drop()
	{
		deltatime = Time.time + timedelay;
		board.clean_piece(piece);
		pos -= new Vector2Int(0, 1);
		board.Move_piece(piece);
    }
}
