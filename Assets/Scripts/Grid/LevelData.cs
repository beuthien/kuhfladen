using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject {

    public List<HexCoordinate> coordinates;
    public List<Tile> tiles;
    public SceneLayout sceneLayout;
}
