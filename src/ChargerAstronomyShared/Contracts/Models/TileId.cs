using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    /// <summary>
    /// Represents a unique identifier for a tile.
    /// </summary>
    public class TileId : IEquatable<TileId> // VS auto completed/added this IEquatable thing, and it seems useful so ill keep it 
    {
        /// <summary>
        /// The unique integer identifier of a tile.
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Initializes a new <see cref="TileId"/> with the specified index.
        /// </summary>
        /// <param name="index">The index representing the unique identifier of the tile.</param>
        public TileId(int index) { Index = index; active = false; }

        /// <summary>
        /// Indicates whether the object is active.
        /// </summary>
        public bool active;

        /// <summary>
        /// Determines whether the current <see cref="TileId"/> is equal to the specified <see cref="TileId"/>.
        /// </summary>
        /// <param name="tileId">The <see cref="TileId"/> to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="TileId"/> has the same <see cref="Index"/> value as the
        /// current instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(TileId tileId) => Index == tileId.Index;

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="TileId"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="TileId"/> instance.</param>
        public override bool Equals(object obj) => obj is TileId tileId && Equals(tileId);

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <returns>An integer representing the hash code for the current object.</returns>
        public override int GetHashCode() => Index.GetHashCode();
    }
}
