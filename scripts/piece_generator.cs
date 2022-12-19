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
		piece r_piece;

		r_piece = piece[Random.Range(0, piece.Length)];
		return (r_piece);
	}
}