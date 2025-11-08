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

        /// <summary>
        /// The total number of tiles.
        /// </summary>
        int TileCount { get; }

        /// <summary>
        /// Iterable list of tiles in this index.
        /// </summary>
        IReadOnlyList<TileId> Tiles { get; }

        /// <summary>
        /// Enumerates the geometry data for all tiles in the collection.
        /// </summary>
        /// <remarks>This method returns a sequence of tuples, where each tuple contains a tile identifier
        /// and its associated geometry data. The enumeration is deferred, meaning the tiles are retrieved lazily as the
        /// sequence is iterated.</remarks>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1, T2}"/> objects, where each tuple consists of a <see
        /// cref="TileId"/> representing the tile identifier and a <see cref="TileGeometry"/> representing the
        /// associated geometry data.</returns>
        IEnumerable<TileId> Enumerate();

        /// <summary>
        /// Retrieves the geometry associated with the specified <see cref="TileId"/>.
        /// </summary>
        /// <param name="id">The identifier of the tile whose geometry is to be retrieved.</param>
        /// <returns>The <see cref="TileGeometry"/> associated with the specified <paramref name="id"/>.</returns>
        /// <exception cref="Exception">Thrown if the geometry for the specified <paramref name="id"/> is not found.</exception>
        IEnumerable<Tuple<TileId, TileGeometry>> EnumerateGeometry();

        /// <summary>
        /// Calculates the center point of the specified tile.
        /// </summary>
        /// <param name="id">The identifier of the tile for which to calculate the center.</param>
        /// <returns>The center point of the tile as a <see cref="Vector3"/>.</returns>
        /// <exception cref="Exception">Thrown if the specified tile identifier does not exist in the tile geometry map.</exception>
        Vector3 GetTileCenter(TileId id);

        /// <summary>
        /// Retrieves the alpha angle for the specified tile.
        /// </summary>
        /// <param name="id">The identifier of the tile for which to retrieve the alpha value.</param>
        /// <returns>Returns the alpha angle from the center of the specified TileId in radians.</returns>
        /// <exception cref="Exception">Thrown if the specified tile is not found in the tile geometry map.</exception>
        double GetTileAlpha(TileId id);

        /// <summary>
        /// Retrieves the neighboring tiles of the specified tile.
        /// </summary>
        /// <param name="id">The identifier of the tile for which to find neighbors.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the identifiers of the neighboring tiles.</returns>
        /// <exception cref="NotImplementedException">This method is not yet implemented.</exception>
        IEnumerable<TileId> Neigbors(TileId id);

        /// <summary>
        /// Retrieves the geometry data associated with the specified tile.
        /// </summary>
        /// <param name="id">The unique identifier of the tile whose geometry data is to be retrieved.</param>
        /// <returns>A <see cref="TileGeometry"/> object representing the geometry of the specified tile. Returns <see
        /// langword="null"/> if the tile does not exist or has no associated geometry.</returns>
        TileGeometry GetGeometry(TileId id);

        /// <summary>
        /// Converts a direction vector to the corresponding TileId on the icosphere.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>The <see cref="TileId"/> in the specified direction.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        TileId DirectionToTileId(Vector3 direction);
    }
}
