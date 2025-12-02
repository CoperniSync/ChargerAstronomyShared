using System;
using System.Collections.Generic;
using System.Numerics;
using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Equatorial;
using ChargerAstronomyShared.Domain.Horizontal;
using ChargerAstronomyShared.Domain.Index;

namespace ChargerAstronomyShared.Domain.SpatialIndex
{
    /// <summary>
    /// Represents a spatial index for organizing and retrieving stars based on their positions in tiles.
    /// </summary>
    /// <remarks>This class provides efficient spatial indexing for stars by associating them with tiles
    /// defined by an <see cref="ITileIndex"/>. Stars can be added individually or in bulk, and their positions are used
    /// to determine the corresponding tile for indexing. The index supports retrieving all stars within a specific tile
    /// and ensures that each star is correctly associated with its corresponding tile.</remarks>
    /// <typeparam name="T">The type of stars to be indexed. Must implement <see cref="IHorizontal"/> to provide positional data.</typeparam>
    public sealed class SpatialStarIndex<T> where T : IHorizontal
    {
        /// <summary>
        /// The TileIndex used for spatial indexing.
        /// </summary>
        public ITileIndex TileIndex { get; }

        /// <summary>
        /// A read-only list of all stars used in the index.
        /// </summary>
        public IReadOnlyList<T> Stars => stars;

        readonly List<T> stars;
        readonly Dictionary<TileId, List<T>> starsByTile = new Dictionary<TileId, List<T>>();
        
        public SpatialStarIndex(ITileIndex tileIndex)
            : this(tileIndex, Array.Empty<T>())
        {
        }

        public SpatialStarIndex(ITileIndex tileIndex, IEnumerable<T> inputStars)
        {
            TileIndex = tileIndex ?? throw new ArgumentNullException(nameof(tileIndex));
            stars = new List<T>(inputStars ?? throw new ArgumentNullException(nameof(inputStars)));

            foreach (var id in tileIndex.Enumerate())
                starsByTile[id] = new List<T>();

            foreach (var star in stars)
            {
                TileId tile = GetTileForStar(star);
                starsByTile[tile].Add(star);
            }
        }

        public void SortAllTilesByMagnitude()
        {
            foreach (var kvp in starsByTile)
            {
                kvp.Value.Sort((a, b) =>
                    a.HorizontalBody.Magnitude.CompareTo(b.HorizontalBody.Magnitude));
            }
        }

        /// <summary>
        /// Adds a new star to the collection and associates it with the appropriate tile.
        /// </summary>
        /// <remarks>The star is added to the main collection and also indexed by its corresponding tile.
        /// If the tile does not already exist in the index, it will be created.</remarks>
        /// <param name="newStar">The star to add. Must not be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newStar"/> is <see langword="null"/>.</exception>
        public void AddStar(T newStar)
        {
            if (newStar == null)
                throw new ArgumentNullException(nameof(newStar));

            stars.Add(newStar);
            TileId tile = GetTileForStar(newStar);

            if (!starsByTile.ContainsKey(tile))
                starsByTile[tile] = new List<T>();

            starsByTile[tile].Add(newStar);

            // sort by magnitude
            starsByTile[tile].Sort((a, b) =>
                a.HorizontalBody.Magnitude.CompareTo(b.HorizontalBody.Magnitude));
        }

        /// <summary>
        /// Adds a collection of stars to the collection and associates it with the appropriate tile.
        /// </summary>
        /// <remarks>Each star is added to the main collection and also indexed by its corresponding tile.
        /// If the tile does not already exist in the index, it will be created.</remarks>
        /// <param name="newStars">A <see cref="PageResult{HorizontalStar}"/> containing the stars to add.  The collection must not be <see
        /// langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newStars"/> is <see langword="null"/>.</exception>
        public void AddStar(PageResult<T> newStars)
        {
            if (newStars == null)
                throw new ArgumentNullException(nameof(newStars));

            foreach (var star in newStars.Items)
            {
                stars.Add(star);
                TileId tile = GetTileForStar(star);

                if (!starsByTile.ContainsKey(tile))
                    starsByTile[tile] = new List<T>();

                starsByTile[tile].Add(star);
            }
        }

        /// <summary>
        /// Returns the <see cref="TileId"/> containing the given star.
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        public TileId GetTileForStar(T star)
        {
            var dir = ToUnitVector(star);
            return TileIndex.DirectionToTileId(dir);
        }

        public IReadOnlyList<T> GetStarsInTile(TileId tile)
        {
            if (!starsByTile.TryGetValue(tile, out var list))
                throw new ArgumentOutOfRangeException(nameof(tile));

            return list;
        }

        /// <summary>
        /// Converts the equatorial coordinates of a star to a unit vector.
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        static Vector3 ToUnitVector(T star)
        {
            var horizontal = star.HorizontalBody;
            double raRad = horizontal.RightAscension * 15.0f *Math.PI / 180.0;
            double decRad = horizontal.Declination * Math.PI / 180.0;

            float x = (float)(Math.Cos(decRad) * Math.Cos(raRad));
            float y = (float)(Math.Cos(decRad) * Math.Sin(raRad));
            float z = (float)(Math.Sin(decRad));

            return Vector3.Normalize(new Vector3(x, y, z));
        }
    }
}
