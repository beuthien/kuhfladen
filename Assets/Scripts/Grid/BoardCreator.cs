using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class BoardCreator : MonoBehaviour {

	public GameObject[] tilePrefabs;
	[SerializeField] GameObject tileSelectionIndicatorPrefab;
	[SerializeField] int width = 10;
	[SerializeField] int height = 8;
	[SerializeField] string levelName = "StartingZone";
	public HexCoordinate pos;
	public SceneLayout sceneLayout = new SceneLayout(2.88675f);
	// [SerializeField] LevelData levelData;
	//public Dictionary<HexCoordinate, GameObject> tiles = new Dictionary<HexCoordinate, GameObject>();
	[SerializeField] Board board = new Board();
	[SerializeField] LevelData levelData;
	
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
		GameObject tile = board.Contains(pos) ? board.GetTileAtCoordinate(pos) : null;
		marker.localPosition = tile != null ? tile.GetComponent<Tile>().center : pos.ToScreenPosition(sceneLayout);
	}

	public void GetDefaultMap()
	{
		for (int r = 0; r < height; r++)
		{
			for (int q = 0; q < width; q++)
			{
				HexCoordinate hexCoordinate = new HexCoordinate(r, q);
				ChangeOrCreate(hexCoordinate, 0);
			}
		}
	}
	
	public void Clear ()
	{
		for (int i = transform.childCount - 1; i >= 0; --i)
			DestroyImmediate(transform.GetChild(i).gameObject);
		board.Empty();
	}
	
	GameObject Create(HexCoordinate hexCoordinate, int selectedIndex)
	{
		Vector3 position = sceneLayout.GetScreenPositionFromHexCoordinate(hexCoordinate);
		GameObject instance = Instantiate(tilePrefabs[selectedIndex], position, Quaternion.identity);
		instance.GetComponent<Tile>().prefabIndex = selectedIndex;
		instance.GetComponent<Tile>().hexPosition = hexCoordinate;
		instance.GetComponent<Tile>().center = position;
		instance.transform.parent = transform;
		return instance;
	}
	
	public GameObject ChangeOrCreate (HexCoordinate hexCoordinate, int selectedIndex)
	{
		GameObject tile = Create(hexCoordinate, selectedIndex);
		if (board.Contains(hexCoordinate))
		{
			GameObject toDelete = board.GetTileAtCoordinate(hexCoordinate);
			board.RemoveTileAtCoordinate(hexCoordinate);
			DestroyImmediate(toDelete);			
		}

		board.AddTileAtCoordinate(tile, hexCoordinate);
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


		LevelData dataToSave = ScriptableObject.CreateInstance<LevelData>();
		dataToSave.tiles = new List<Tile>();
		dataToSave.coordinates = new List<HexCoordinate>();
		dataToSave.sceneLayout = sceneLayout;
		foreach (var element in board.tiles)
		{
			dataToSave.tiles.Add(element.Value.GetComponent<Tile>());
			dataToSave.coordinates.Add(element.Key);
		}
		string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, levelName);
		Debug.Log("Save to file " + fileName);
		AssetDatabase.CreateAsset(dataToSave, fileName);
	}

	public void Load ()
	{
		Clear();
		if (levelData == null)
			return;
		Debug.Log("Load level" + levelData.ToString());	
		AssetDatabase.Refresh();
		sceneLayout = levelData.sceneLayout;
		for (int i = 0; i < levelData.coordinates.Count; i++)
		{
			int index = levelData.tiles[i].prefabIndex;
			HexCoordinate hexCoordinate = levelData.coordinates[i];
			Vector3 position = sceneLayout.GetScreenPositionFromHexCoordinate(hexCoordinate);
			GameObject instance = Instantiate(tilePrefabs[index], position, Quaternion.identity);
			instance.transform.parent = transform;
			board.AddTileAtCoordinate(instance, levelData.coordinates[i]);
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
