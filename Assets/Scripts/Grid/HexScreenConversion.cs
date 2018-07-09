using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HexScreenConversion
{
    // Element order: a11 a12 a21 a22
    private readonly Vector4 hexToScreenMatrixEntries;
    private readonly Vector4 screenToHexMatrixEntries;

    public HexScreenConversion(
        float hexScreen11, float hexScreen12, float hexScreen21, float hexScreen22,
        float screenHex11, float screenHex12, float screenHex21, float screenHex22)
    {
        hexToScreenMatrixEntries = new Vector4(hexScreen11, hexScreen12, hexScreen21, hexScreen22);
        screenToHexMatrixEntries = new Vector4(screenHex11, screenHex12, screenHex21, screenHex22);
    }

    public Vector3 ConvertHexCoordinateToScreenPosition(HexCoordinate hexCoordinate)
    {
        float x = hexToScreenMatrixEntries.x * hexCoordinate.r + hexToScreenMatrixEntries.y * hexCoordinate.q;
        float z = hexToScreenMatrixEntries.z * hexCoordinate.r + hexToScreenMatrixEntries.w * hexCoordinate.q;
        return new Vector3(x, 0f, z);
    }
    
    public HexCoordinate ConvertScreenPositionToHexCoordinate(Vector3 screenPosition)
    {
        float fracR = screenToHexMatrixEntries.x * screenPosition.x + screenToHexMatrixEntries.y * screenPosition.z;
        float fracQ = screenToHexMatrixEntries.z * screenPosition.x + screenToHexMatrixEntries.w * screenPosition.z;
        Debug.Log("Postion = " + screenPosition + "r = " + Mathf.RoundToInt(fracR) + ", q = " + Mathf.RoundToInt(fracQ));
        return RoundCoordinates(fracR, fracQ);
    }

    public HexCoordinate RoundCoordinates(float fracR, float fracQ)
    {
        return new HexCoordinate(Mathf.RoundToInt(fracR), Mathf.RoundToInt(fracQ));
    }
}
