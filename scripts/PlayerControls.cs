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
	private float		locktime;
	public float 		timelock;
    public string		Dir_src;

    void Update()
    {
		if (Time.time >= deltatime)
		{
			board.clean_piece(piece);
			if (board.IsValid(new Vector2Int(0, -1), piece))
				Drop();
			else
			{
				board.Move_piece(piece);
				new_piece();
			}
		}
		else if (Input.GetAxis(Dir_src) != 0 && Time.time >= locktime)
			player_move();
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
		pos += new Vector2Int(0, -1);
		board.Move_piece(piece);
    }

	public void player_move()
	{
		int 		dir;
		Vector2Int 	V;

		dir = 1;
		board.clean_piece(piece);
		if (Input.GetAxis(Dir_src) < 0)
			dir = -1;
		V = new Vector2Int(dir, 0);
		if (board.IsValid(V, piece))
			pos += V;
		board.Move_piece(piece);
		locktime = Time.time + timelock;
	}
}
