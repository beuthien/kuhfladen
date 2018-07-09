using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor 
{
	public BoardCreator current
	{
		get
		{
			return (BoardCreator)target;
		}
	}
	
	public int selectedIndex = 0;
	bool enableEditMode = false;

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		string[] prefabNames = new string[current.tilePrefabs.Length];
		for (int i = 0; i < current.tilePrefabs.Length; i++)
		{
			prefabNames[i] = current.tilePrefabs[i].name;
		}

		enableEditMode = GUILayout.Toggle(enableEditMode, "Editing on");
		selectedIndex = GUILayout.SelectionGrid(selectedIndex, prefabNames, 1);
		
		if (GUILayout.Button("Clear"))
			current.Clear();

		if (GUILayout.Button("Default"))
		{
			current.GetDefaultMap();
		}

		if (GUILayout.Button("Save"))
		{
			current.Save();
		}

		if (GUILayout.Button("Load"))
		{
			current.Load();
		}
		
		if (GUI.changed)
			current.UpdateMarker ();
	}
	
	void OnSceneGUI()
	{
		//Debug.Log(screenTilePosition);
		//if 'E' pressed, spawn the selected prefab
		if (enableEditMode && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.I)
		{
			
			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			Vector3 point = ComputeYZeroCrossing(ray);
			Spawn(point);
		}

		if (Event.current.rawType == EventType.MouseDrag)
		{
			Debug.Log("MouseDrag");
			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			Vector3 point = ComputeYZeroCrossing(ray);			
			HexCoordinate hexCoordinate = current.sceneLayout.GetHexCoordinateFromScenePosition(point);
			current.pos = hexCoordinate;
			current.UpdateMarker();
		}
	}

	private Vector3 ComputeYZeroCrossing(Ray ray)
	{
		// 0 = ox + alpha dy
		float alpha = -ray.origin.y / ray.direction.y;
		return ray.origin + alpha * ray.direction;
	}

	void Spawn(Vector3 screenTilePosition)
	{
		Debug.Log("Spawn: Position tile at " + screenTilePosition);
		HexCoordinate hexCoordinate = current.sceneLayout.GetHexCoordinateFromScenePosition(screenTilePosition);
		Vector3 adjustedTilePosition = current.sceneLayout.GetScreenPositionFromHexCoordinate(hexCoordinate);
		current.ChangeOrCreate(hexCoordinate, selectedIndex);
	}
}
