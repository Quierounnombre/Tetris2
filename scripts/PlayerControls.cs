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
	public int			RotationIndex;

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

private void Rotate(int direction)
    {
        int originalRotation = rotationIndex;

        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);
        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = piece.RotationMatrix;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3 cell = piece.cells[i];

            int x, y;

			if (piecei.shape == O)
			{
				cell.x -= 0.5f;
				cell.y -= 0.5f;
				x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
				y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
			}
			if (piece.shape != T)
			{
				x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
				y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
            }
            cells[i] = new Vector3Int(x, y, 0);
        }
    }


    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }
}
