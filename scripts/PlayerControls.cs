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
	public string		Rot_src;
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
		else if (Input.GetAxis(Rot_src) != 0 && Time.time >= locktime)
			Rotate(Wrap((int)Input.GetAxis(Rot_src), 0, 1));
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
        int originalRotation = RotationIndex;

		board.clean_piece(piece);
		RotationIndex = Wrap(RotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);
        if (!TestWallKicks(RotationIndex, direction))
        {
			RotationIndex = originalRotation;
			ApplyRotationMatrix(-direction);
        }
		board.Move_piece(piece);
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = piece.RotationMatrix;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3 cell = (Vector3)(Vector3Int)piece.cells[i];

            int x = 0, y = 0;

			if (piece.shape == shape.O)
			{
				cell.x -= 0.5f;
				cell.y -= 0.5f;
				x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
				y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
			}
			if (piece.shape != shape.I)
			{
				x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
				y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
            }
            piece.cells[i] = new Vector2Int(x, y);
        }
    }

	public bool TestWallKicks(int RotationIndex, int RotationDirection)
	{
		int WallIndex = GetWallIndex(RotationIndex, RotationDirection);

		for (int i = 0; i < piece.wallkicks.GetLength(1); i++)
		{
			Vector2Int translation = piece.wallkicks[WallIndex, i];
			if (board.IsValid(translation, piece))
				return (true);
		}
		return (false);
	}

	private int GetWallIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, piece.wallkicks.GetLength(0));
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
