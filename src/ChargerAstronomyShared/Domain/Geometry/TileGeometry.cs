using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Geometry
{
    using ChargerAstronomyShared.Contracts.Models;

    /// <summary>
    /// Information about the geometry of a tile.
    /// </summary>
    public readonly struct TileGeometry
    {

        /// <summary>
        /// The unique identifier of the tile.
        /// </summary>
        public readonly TileId Id { get; }

        /// <summary>
        /// The vector pointing to the center of the tile.
        /// </summary>
        public readonly Vector3 Center { get; }

        /// <summary>
        /// The alpha angle (in radians) from the center of the tile. Used in calculations.
        /// </summary>
        public readonly double Alpha { get; }

        /// <summary>
        /// The list of vertices that define the tile, in counter-clockwise order.
        /// </summary>
        public readonly List<Vector3> Vertices { get; } // in counter-clockwise order

        /// <summary>
        /// Initializes a new instance of the <see cref="TileGeometry"/> class with the specified tile identifier,
        /// center position, rotation angle, and vertex list.
        /// </summary>
        /// <param name="id">The unique identifier of the tile.</param>
        /// <param name="center">The center position of the tile in 3D space.</param>
        /// <param name="alpha">The rotation angle of the tile, in radians.</param>
        /// <param name="vertices">The list of vertices defining the geometry of the tile. Cannot be null.</param>
        public TileGeometry(TileId id, Vector3 center, double alpha, List<Vector3> vertices)
        {
            Id = id;
            Center = center;
            Alpha = alpha;
            Vertices = vertices;
        }

        /// <summary>
        /// Returns a string representation of the tile geometry, including its identifier and alpha value.
        /// </summary>
        /// <returns>A string in the format "TileGeometry(Id={Id}, Alpha={Alpha})", where <c>Id</c> is the tile's identifier and
        /// <c>Alpha</c> is the alpha value formatted to three decimal places.</returns>
        public override string ToString() => $"TileGeometry(Id={Id.Index}, Alpha={Alpha:0.###})";
    }
}
