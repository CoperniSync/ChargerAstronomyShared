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

    public static List<TileId> Select(
        ITileIndex index,
        Vector3 cameraDirection,
        float fov,               // radians
        bool contains = false,
        float eps = 1e-6f)
    {
        cameraDirection = Vector3.Normalize(cameraDirection);
        float halfFov = fov * 0.5f;

        var selected = new List<TileId>();

        foreach (var (tileId, geom) in index.EnumerateGeometry())
        {
            float alpha = (float)geom.Alpha;         
            float dot = Vector3.Dot(cameraDirection, geom.Center); 

            if (!contains)
            {
                float limit = MathF.Cos(halfFov + alpha);
                if (dot >= limit - eps) selected.Add(tileId);
            }
            else
            {
                float inner = halfFov - alpha;
                if (inner <= 0f)
                {
                    // here the camera fov is smaller than the tile
                    // might happen if you are zoomed in realllly close
                    // we treat it as an intersection
                    selected.Add(tileId);
                }
                else
                {
                    float limit = MathF.Cos(inner);
                    if (dot >= limit - eps) selected.Add(tileId);
                }
            }
        }

        return selected;
    }
}
