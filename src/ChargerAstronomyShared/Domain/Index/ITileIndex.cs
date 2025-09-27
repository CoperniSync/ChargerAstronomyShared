using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain.Index
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Geometry;

    public interface ITileIndex
    {
        int TileCount { get; }
        IEnumerable<TileId> Enumerate();
        Vector3 GetTileCenter(TileId id);
        double GetTileAlpha(TileId id);
        IEnumerable<TileId> Neigbors(TileId id);
        TileId DirectionToTileId(Vector3 direction);
    }
}
