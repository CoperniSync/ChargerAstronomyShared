using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Index
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Geometry;

    public interface ITileIndex
    {
        int TileCount { get; }
        IReadOnlyList<TileId> Tiles { get; }
        IEnumerable<TileId> Enumerate();
        IEnumerable<Tuple<TileId, TileGeometry>> EnumerateGeometry();
        Vector3 GetTileCenter(TileId id);
        double GetTileAlpha(TileId id);
        IEnumerable<TileId> Neigbors(TileId id);
        TileGeometry GetGeometry(TileId id);
        TileId DirectionToTileId(Vector3 direction);
    }
}
