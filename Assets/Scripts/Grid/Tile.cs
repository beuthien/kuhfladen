using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public HexCoordinate hexPosition;
	public SceneLayout sceneLayout;
	public Vector3 center;
	public int prefabIndex;
	
	// Use this for initialization
	void Start () {
		hexPosition = new HexCoordinate(0, 0);
		sceneLayout = new SceneLayout(gameObject);
		center = sceneLayout.GetScreenPositionFromHexCoordinate(hexPosition);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
