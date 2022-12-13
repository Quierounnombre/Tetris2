using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class board : MonoBehaviour
{
	public Tilemap tilemap;
	public piece_generator gen;
	private Tile tile;
	public PlayerControls player;
	private void Start() {
		piece p = gen.generate();
		Move_piece(p);
	}

	
    public void Move_piece(piece piece)
    {
		Vector2Int tilepos;

        for (int i = 0; i < piece.cells.Length; i++)	
        {
			tilepos = piece.cells[i] + player.pos;
			Debug.Log((Vector3Int)tilepos);
			tilemap.SetTile((Vector3Int)tilepos, piece.tile);
        }
		Debug.Log("He terminado");
    }
	
}