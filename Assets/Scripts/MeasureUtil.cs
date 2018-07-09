using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureUtil : MonoBehaviour
{

	public Vector3 sizeOfObject;
	public Vector3 center;
	
	private void Start()
	{
		MeshRenderer renderer = gameObject.GetComponent < MeshRenderer >();
		sizeOfObject = renderer.bounds.size;
		center = renderer.bounds.center;
	}

}
