using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This struct provides an axial coordinate representation for hexagonal grids. 
// See https://www.redblobgames.com/grids/hexagons/#basics for a nice overview of it.
[System.Serializable]
public class HexCoordinate
{
	public int r;
	public int q;

	public HexCoordinate(int r, int q)
	{
		this.r = r;
		this.q = q;
	}

	public Vector3 ToScreenPosition(SceneLayout sceneLayout)
	{
		return sceneLayout.GetScreenPositionFromHexCoordinate(this);
	}
	
	public static HexCoordinate operator +(HexCoordinate a, HexCoordinate b)
	{
		return new HexCoordinate(a.r + b.r, a.q + b.q);
	}
	
	public static HexCoordinate operator -(HexCoordinate p1, HexCoordinate p2) 
	{
		return new HexCoordinate(p1.r - p2.r, p1.q - p2.q);
	}
	
	public static bool operator ==(HexCoordinate a, HexCoordinate b)
	{
		return a.r == b.r && a.q == b.q;
	}
	
	public static bool operator !=(HexCoordinate a, HexCoordinate b)
	{
		return !(a == b);
	}
	
	public override bool Equals (object obj)
	{
		if (obj is HexCoordinate)
		{
			HexCoordinate p = (HexCoordinate)obj;
			return r == p.r && q == p.q;
		}
		return false;
	}
	public bool Equals (HexCoordinate p)
	{
		return r == p.r && q == p.q;
	}
	public override int GetHashCode ()
	{
		return r ^ q;
	}

	public override string ToString()
	{
		return string.Format ("({0},{1})", r, q);;
	}
}
