using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	[Header ("Estado del tablero")]
	public Background_color	bg_color;
	public bool				is_P1;
	public bool				can_P1_drop;
	public int				RotationIndex;
    public GameManager 		GameManager;
	public board			board;
	public PlayerControls	other_player;
	public piece			piece;
    public Vector2Int		pos;
    public float       		timedelay;
    private float   	    deltatime;
	public float			time_reduction;
	private float			locktime;
	public float 			timelock;
	[Space(10)]
	[Header("Jugador 1 controles")]
    public string			Dir_pos_src1;
	public string			Dir_neg_src1;
	public string			Rot_pos_src1;
	public string			Rot_neg_src1;
	public string			drop_src1;
	[Space(10)]
	[Header ("Jugador 2 controles")]
	public string			Dir_pos_src2;
	public string			Dir_neg_src2;
	public string			Rot_pos_src2;
	public string			Rot_neg_src2;
	public string			drop_src2;

    void LateUpdate()
    {
		if (is_P1)
			move_p1();
		else
			move_p2();
    }

	private void move_p1()
	{
		if (Time.time >= locktime)
		{
			if (Input.GetKeyDown(drop_src1) && can_P1_drop)
				Hard_drop();
			else if (Input.GetKeyDown(Dir_pos_src1) )
				player_move(1);
			else if (Input.GetKeyDown(Dir_neg_src1))
				player_move(-1);
			else if (Input.GetKeyDown(Rot_pos_src1))
				Rotate(1);
			else if (Input.GetKeyDown(Rot_neg_src1))
				Rotate(-1);
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
		if (Time.time >= locktime)
		{
			if (Input.GetKeyDown(drop_src2) && !can_P1_drop)
				Hard_drop();
			else if (Input.GetKeyDown(Dir_pos_src2) )
				player_move(1);
			else if (Input.GetKeyDown(Dir_neg_src2))
				player_move(-1);
			else if (Input.GetKeyDown(Rot_pos_src2))
				Rotate(1);
			else if (Input.GetKeyDown(Rot_neg_src2))
				Rotate(-1);
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
		bg_color.color_swap(is_P1);
		Check_hard_drop();
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
		new_piece();
	}

	public void player_move(int dir)
	{
		Vector2Int 	V;

		board.clean_piece(piece);
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

	private void	Check_hard_drop()
	{
		if (is_P1)
			can_P1_drop = true;
		else
			can_P1_drop = false;
		if (other_player.is_P1)
			other_player.can_P1_drop = true;			
		else
			other_player.can_P1_drop = false;
		is_P1 = !is_P1;
	}
}
