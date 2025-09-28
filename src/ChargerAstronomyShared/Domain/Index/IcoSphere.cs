using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain.Index
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Geometry;

    public sealed class IcoSphere : ITileIndex
    {
        public int TileCount => throw new NotImplementedException();

        public TileId DirectionToTileId(Vector3 direction)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TileId> Enumerate()
        {
            throw new NotImplementedException();
        }

        public double GetTileAlpha(TileId id)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetTileCenter(TileId id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TileId> Neigbors(TileId id)
        {
            throw new NotImplementedException();
        }
    }
}
