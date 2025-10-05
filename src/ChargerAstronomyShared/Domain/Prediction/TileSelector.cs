using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Prediction
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Geometry;
    using ChargerAstronomyShared.Domain.Index;

    public static class TileSelector
    {
        public static List<TileId> Select(
            ITileIndex index, 
            Vector3 cameraDirection, 
            float fov,
            bool contains = false,
            float eps = 1e-6f)
        {
            cameraDirection = Vector3.Normalize(cameraDirection);

            List<TileId> selected = new List<TileId>();
            foreach((TileId tileId, TileGeometry tileGeometry) in index.EnumerateGeometry())
            {
                float halfFov = fov / 2.0f;
                float dot = Vector3.Dot(cameraDirection, tileGeometry.Center);
               
                if(contains)
                {
                    if(dot >= Math.Cos(halfFov - tileGeometry.Alpha + eps))
                        selected.Add(tileId);
                }
                else
                {
                    if (dot >= Math.Cos(halfFov + tileGeometry.Alpha + eps))
                        selected.Add(tileId);
                }

            }    

            return selected;
        }
    }
}
