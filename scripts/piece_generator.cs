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
}


public class piece_generator : MonoBehaviour 
{
	public Tilemap	tilemap_p1;
	public Tilemap	tilemap_p2;
	public piece[]	piece;

	public piece generate()
	{
		piece r_piece;

		r_piece = piece[Random.Range(0, piece.Length)];
		r_piece.cells = data.cells[r_piece.shape];
		return (r_piece);
	}
}