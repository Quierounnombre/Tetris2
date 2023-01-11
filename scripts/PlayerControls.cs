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
	public float		rot_delay;
    private float       deltatime;
	private float		locktime;
	private float		rot_locktime;
	public float 		timelock;
    public string		Dir_src1;
	public string		Rot_src1;
	public string		drop_src1;
	public string		drop_src2;
	public string		Dir_src2;
	public string		Rot_src2;
	public bool			is_P1;
	public int			RotationIndex;

    void Update()
    {
		if (is_P1)
			move_p1();
		else
			move_p2();
    }

	private void move_p1()
	{
		if (Input.GetKeyDown(drop_src1))
			Hard_drop();
		else if (Input.GetAxis(Dir_src1) != 0 && Time.time >= locktime)
			player_move(Input.GetAxis(Dir_src1));
		else if (Input.GetAxis(Rot_src1) != 0 && Time.time >= locktime && Time.time >= rot_locktime)
		{
			if (Input.GetAxis(Rot_src1) > 0)
				Rotate(1);
			else
				Rotate(-1);
			rot_locktime = Time.time + rot_delay;
		}
		if (Time.time >= deltatime)
		{
			board.clean_piece(piece);
			if (board.IsValid(new Vector2Int(0, -1), piece))
				Drop();
			else
			{
				board.Move_piece(piece);
				is_P1 = false;
				new_piece();
			}
		}
	}
	private void move_p2()
	{
		if (Input.GetKeyDown(drop_src2))
			Hard_drop();
		else if (Input.GetAxis(Dir_src2) != 0 && Time.time >= locktime)
			player_move(Input.GetAxis(Dir_src2));
		else if (Input.GetAxis(Rot_src2) != 0 && Time.time >= locktime && Time.time >= rot_locktime)
		{
			if (Input.GetAxis(Rot_src2) > 0)
				Rotate(1);
			else
				Rotate(-1);
			rot_locktime = Time.time + rot_delay;
		}
		if (Time.time >= deltatime)
		{
			board.clean_piece(piece);
			if (board.IsValid(new Vector2Int(0, -1), piece))
				Drop();
			else
			{
				board.Move_piece(piece);
				is_P1 = true;
				new_piece();
			}
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
		pos += new Vector2Int(0, -1);
		board.Move_piece(piece);
    }

	public void Hard_drop()
	{
		while (true)
		{
			board.clean_piece(piece);
			if (board.IsValid(new Vector2Int(0, -1), piece))
			{
				pos += new Vector2Int(0, -1);
				board.Move_piece(piece);
			}
			else
				break;
		}
		board.Move_piece(piece);
		is_P1 = !is_P1;
		new_piece();
		deltatime = Time.time + timedelay;
	}

	public void player_move(float input)
	{
		int 		dir;
		Vector2Int 	V;

		dir = 1;
		board.clean_piece(piece);
		if (input < 0)
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

			int x, y;
            switch (piece.shape)
            {
                case shape.I:
                case shape.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
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
			if (!board.IsValid(translation, piece))
				return (false);
		}
		return (true);
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
        if (input < min)
		{
            return max - (min - input) % (max - min);
        }
		else
		{
            return min + (input - min) % (max - min);
        }
    }
}
