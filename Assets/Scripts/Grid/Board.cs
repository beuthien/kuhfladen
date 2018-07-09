using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
	public Dictionary<HexCoordinate, GameObject> tiles;

	public Board()
	{
		tiles = new Dictionary<HexCoordinate, GameObject>();
	}
	
	public bool Contains(HexCoordinate hexCoordinate)
	{
		return tiles.ContainsKey(hexCoordinate);
	}

	public GameObject GetTileAtCoordinate(HexCoordinate hexCoordinate)
	{
		return tiles[hexCoordinate];
	}

	public void Empty()
	{
		tiles.Clear();
	}

	public void RemoveTileAtCoordinate(HexCoordinate hexCoordinate)
	{
		tiles.Remove(hexCoordinate);
	}


	public void AddTileAtCoordinate(GameObject tile, HexCoordinate hexCoordinate)
	{
		tiles.Add(hexCoordinate, tile);
	}
}
