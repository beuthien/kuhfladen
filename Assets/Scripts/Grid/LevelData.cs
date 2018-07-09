using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject {

    public Dictionary<HexCoordinate, Tile> tiles = new Dictionary<HexCoordinate, Tile>();

    public bool Contains(HexCoordinate hexCoordinate)
    {
        return tiles.ContainsKey(hexCoordinate);
    }
    
    
}
