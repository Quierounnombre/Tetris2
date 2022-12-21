using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum shape
{
	T,
	I,
	O,
	S,
	Z,
	J,
	L,
}

[System.Serializable]
public struct piece
{
	public Tile tile;
	public shape shape;
	public Vector2Int[] cells;
	public Vector2Int[,] wallkicks;
	public float[] RotationMatrix;
}


public class piece_generator : MonoBehaviour 
{
	public piece[]	piece;

	private void Awake()
	{
		int		i;
		float	cos;
		float	sin;		

		cos = Mathf.Cos(Mathf.PI / 2f);
		sin = Mathf.Sin(Mathf.PI / 2f);
		i = 0;
		while (i < piece.Length)
		{
			piece[i].cells = data.cells[piece[i].shape];
			piece[i].RotationMatrix = new float[] {cos, sin, -sin, cos};
			piece[i].wallkicks = data.WallKicks[piece[i].shape];
			i++;
		}
	}

	public piece generate()
	{
		piece	r_piece;
		piece	n_piece;

		r_piece = piece[Random.Range(0, piece.Length)];
		n_piece.shape = r_piece.shape;
		n_piece.RotationMatrix = r_piece.RotationMatrix;
		n_piece.tile = r_piece.tile;
		n_piece.cells = (Vector2Int[])r_piece.cells.Clone();
		n_piece.wallkicks = r_piece.wallkicks;
		return (n_piece);
	}
}