using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;

// The (pointy-top) grid layout in the scene. The class is responsible for taking care of the tile sizes etc.
public class SceneLayout
{
	public float size;
	
	// flat-top
	private readonly HexScreenConversion conversion = 
		new HexScreenConversion(0.0f, 3.0f / 2.0f, Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 
			-1.0f/3.0f, Mathf.Sqrt(3f)/3f, 2.0f/3.0f, 0f);
	
	// Matrix indices for pointy lauyout
	//public HexScreenConversion conversion = 
	//	new HexScreenConversion(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, 
	//		Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f);

	public SceneLayout(GameObject tile)
	{
		// In ppoint-top size might be determied via the height (over x-coordinate), i.e. h = 2 * size. 
		size = tile.GetComponent<MeshRenderer>().bounds.size.x / 2f;
	}

	public SceneLayout(float size)
	{
		this.size = size;
	}

	public Vector3 GetScreenPositionFromHexCoordinate(HexCoordinate hexCoordinate)
	{
		return size * conversion.ConvertHexCoordinateToScreenPosition(hexCoordinate);
	}

	public HexCoordinate GetHexCoordinateFromScenePosition(Vector3 scenePosition)
	{
		Debug.Log("SceneLayout Point = " + scenePosition);
		return conversion.ConvertScreenPositionToHexCoordinate(scenePosition / size);
	}	
}
