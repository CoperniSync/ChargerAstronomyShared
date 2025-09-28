using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Geometry
{
    using ChargerAstronomyShared.Contracts.Models;

    //<summary>
     
    //<summary>

    public readonly struct TileGeometry
    {
        public readonly TileId Id { get; }

        public readonly Vector3 Center { get; }

        public readonly double Alpha { get; }

        public readonly List<Vector3> Vertices { get; } // in counter-clockwise order

        public TileGeometry(TileId id, Vector3 center, double alpha, List<Vector3> vertices)
        {
            Id = id;
            Center = center;
            Alpha = alpha;
            Vertices = vertices;
        }

        public override string ToString() => $"TileGeometry(Id={Id.Index}, Alpha={Alpha:0.###})";
    }
}
