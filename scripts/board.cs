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
	public Vector2Int spawn_point;

    public void Move_piece(piece piece)
    {
		Vector2Int tilepos;

        for (int i = 0; i < piece.cells.Length; i++)	
        {
			tilepos = piece.cells[i] + player.pos;
			tilemap.SetTile((Vector3Int)tilepos, piece.tile);
        }
    }

	public void clean_piece(piece piece)
	{
		Vector2Int tilepos;

        for (int i = 0; i < piece.cells.Length; i++)	
        {
			tilepos = piece.cells[i] + player.pos;
			tilemap.SetTile((Vector3Int)tilepos, null);
        }
	}
	
	public bool IsValid(Vector2Int offset, piece piece)
	{
		Vector2Int	targetpos;
		Vector2Int	cellpos;

		targetpos = player.pos + offset;
		for (int i = 0; i < piece.cells.Length; i++)
		{
			cellpos = piece.cells[i] + targetpos;
			if (tilemap.HasTile((Vector3Int)cellpos))
				return (false);
			if (cellpos.x < 0 || cellpos.y >= 20 || cellpos.y < 0 || cellpos.x >= 10)
				return (false);
		}
		return (true);
	}

	public void spawn_piece(piece piece)
	{
		Vector2Int tilepos;

		for (int i = 0; i < piece.cells.Length; i++)
		{
			tilepos = piece.cells[i] + spawn_point;
			if (tilemap.GetTile((Vector3Int)tilepos) != null)
				Time.timeScale=0; // GAMEOVER IN FUTURE
		}
		player.pos = spawn_point;
		Move_piece(piece);
	}
	
}