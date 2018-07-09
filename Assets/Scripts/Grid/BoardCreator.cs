using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class BoardCreator : MonoBehaviour {

	public GameObject[] tilePrefab;
	[SerializeField] GameObject tileSelectionIndicatorPrefab;
	[SerializeField] int width = 10;
	[SerializeField] int depth = 10;
	[SerializeField] int height = 8;
	public HexCoordinate pos;
	private SceneLayout sceneLayout = new SceneLayout(2.88675f);
	// [SerializeField] LevelData levelData;
	public Dictionary<HexCoordinate, GameObject> tiles = new Dictionary<HexCoordinate, GameObject>();
	//public Board board;
	
	Transform marker
	{
		get
		{
			if (_marker == null)
			{
				GameObject instance = Instantiate(tileSelectionIndicatorPrefab);
				_marker = instance.transform;
			}
			return _marker;
		}
	}
	Transform _marker;
	
	public void UpdateMarker ()
	{
		GameObject tile = tiles.ContainsKey(pos) ? tiles[pos] : null;
		marker.localPosition = tile != null ? tile.GetComponent<Tile>().center : pos.ToScreenPosition(sceneLayout);
	}

	public void GetDefaultMap()
	{
		for (int r = 0; r < height; r++)
		{
			for (int q = 0; q < width; q++)
			{
				HexCoordinate hexCoordinate = new HexCoordinate(r, q);
				Debug.Log(hexCoordinate.ToString());
				GetOrCreate(hexCoordinate, 0);
			}
		}
	}
	
	public void Clear ()
	{
		for (int i = transform.childCount - 1; i >= 0; --i)
			DestroyImmediate(transform.GetChild(i).gameObject);
		tiles.Clear();
	}
	
	GameObject Create(Vector3 position, int selectedIndex)
	{
		GameObject instance = Instantiate(tilePrefab[selectedIndex], position, Quaternion.identity);
		instance.transform.parent = transform;
		return instance;
	}
	
	public GameObject GetOrCreate (HexCoordinate hexCoordinate, int selectedIndex)
	{
		GameObject tile = Create(sceneLayout.GetScreenPositionFromHexCoordinate(hexCoordinate), selectedIndex);
		if (tiles.ContainsKey(hexCoordinate))
		{
			GameObject toDelete = tiles[hexCoordinate];
			tiles.Remove(hexCoordinate);
			DestroyImmediate(toDelete);			
		}
		tiles.Add(hexCoordinate, tile);
		return tile;
	}

	HexCoordinate SelectPosition()
	{
		return new HexCoordinate(0, 0);
	}
	
	public void Save ()
	{
		string filePath = Application.dataPath + "/Resources/Levels";
		if (!Directory.Exists(filePath))
			CreateSaveDirectory ();
		
		LevelData board = ScriptableObject.CreateInstance<LevelData>();
		board.tiles = new List<Vector3>( tiles.Count );
		foreach (Tile t in tiles.Values)
			board.tiles.Add( new Vector3(t.pos.x, t.height, t.pos.y) );
		
		string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
		AssetDatabase.CreateAsset(board, fileName);
	}

	public void Load ()
	{
		Clear();
		if (levelData == null)
			return;
		
		foreach (Vector3 v in levelData.tiles)
		{
			Tile t = Create();
			t.Load(v);
			tiles.Add(t.pos, t);
		}
	}
	
	void CreateSaveDirectory ()
	{
		string filePath = Application.dataPath + "/Resources";
		if (!Directory.Exists(filePath))
			AssetDatabase.CreateFolder("Assets", "Resources");
		filePath += "/Levels";
		if (!Directory.Exists(filePath))
			AssetDatabase.CreateFolder("Assets/Resources", "Levels");
		AssetDatabase.Refresh();
	}
}
