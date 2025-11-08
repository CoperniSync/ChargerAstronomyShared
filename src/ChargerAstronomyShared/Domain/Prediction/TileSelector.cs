using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Index;
using System.Collections.Generic;
using System.Numerics;
using System;

public static class TileSelector
{
    // This is a really basis predictor for now, just does camera tile intsection
    // a more sophisticated predictor will be needed later
    // also its assumed that the cameraDirection is in equatorial space

    /// <summary>
    /// Selects tiles from the given tile index that intersect with the specified camera direction and field of view
    /// (FOV).
    /// </summary>
    /// <remarks>This method normalizes the <paramref name="cameraDirection"/> vector before performing
    /// calculations.  The selection process accounts for the angular size of each tile and the specified FOV.  If the
    /// FOV is smaller than the angular size of a tile, the tile is treated as intersecting the FOV.</remarks>
    /// <param name="index">The tile index containing the tiles and their associated geometry.</param>
    /// <param name="cameraDirection">The direction of the camera in equatorial space. Must be a non-zero vector.</param>
    /// <param name="fov">The field of view (FOV) in radians. Must be a positive value.</param>
    /// <param name="scratch">A list to store the selected tiles. The method appends results to this list.</param>
    /// <param name="contains">A boolean indicating whether to use a "contains" mode for selection.  If <see langword="false"/>, tiles are
    /// selected if they intersect with the FOV.  If <see langword="true"/>, tiles are selected if they are fully
    /// contained within the FOV.</param>
    /// <param name="eps">A small epsilon value used for numerical precision adjustments. Defaults to 1e-6.</param>
    /// <returns>A list of tile IDs representing the tiles that intersect with or are contained within the specified FOV, 
    /// depending on the value of <paramref name="contains"/>. The returned list is the same as the <paramref
    /// name="scratch"/> list.</returns>
    public static List<TileId> Select(
        ITileIndex index,
        Vector3 cameraDirection,
        float fov,               // radians
        List<TileId> scratch,
        bool contains = false,
        float eps = 1e-6f
        )
    {
        cameraDirection = Vector3.Normalize(cameraDirection);
        float halfFov = fov * 0.5f;

        foreach (var (tileId, geom) in index.EnumerateGeometry())
        {
            float alpha = (float)geom.Alpha;         
            float dot = Vector3.Dot(cameraDirection, geom.Center); 

            if (!contains)
            {
                float limit = MathF.Cos(halfFov + alpha);
                if (dot >= limit - eps) scratch.Add(tileId);
            }
            else
            {
                float inner = halfFov - alpha;
                if (inner <= 0f)
                {
                    // here the camera fov is smaller than the tile
                    // might happen if you are zoomed in realllly close
                    // we treat it as an intersection
                    scratch.Add(tileId);
                }
                else
                {
                    float limit = MathF.Cos(inner);
                    if (dot >= limit - eps) scratch.Add(tileId);
                }
            }
        }

        return scratch;
    }
}
