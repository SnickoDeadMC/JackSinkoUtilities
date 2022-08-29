using UnityEngine;

public static class Vector3Extensions
{

    public static GridPosition ToGridPos(this Vector3 vector3)
    {
        long xGrid = Mathf.RoundToInt(vector3.x) / GridPosition.TileSize;
        long zGrid = Mathf.RoundToInt(vector3.z) / GridPosition.TileSize;
        return new GridPosition(xGrid, zGrid);
    }
    
}
