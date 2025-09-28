using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    using ChargerAstronomyShared.Domain.Geometry;

    public readonly struct TileMeta
    {
        public readonly TileId Id;

        public readonly Vector3 Center;

        public readonly double Alpha; // this will be used for prediction

        public readonly int Count; // the number of stars in this tile

        public TileMeta(TileId id, Vector3 center, double alpha, int count)
        {
            Id = id;
            Center = center;
            Alpha = alpha;
            Count = count;
        }
    }
}
