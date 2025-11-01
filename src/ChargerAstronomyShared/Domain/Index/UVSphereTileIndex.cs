using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ChargerAstronomyShared.Domain.Index
{
    public class UVSphereTileIndex : ITileIndex
    {
        public int TileCount => throw new NotImplementedException();

        public IReadOnlyList<TileId> Tiles => throw new NotImplementedException();

        public TileId DirectionToTileId(Vector3 direction)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TileId> Enumerate()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<TileId, TileGeometry>> EnumerateGeometry()
        {
            throw new NotImplementedException();
        }

        public TileGeometry GetGeometry(TileId id)
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
