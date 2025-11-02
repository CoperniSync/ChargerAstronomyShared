using System;
using System.Collections.Generic;
using System.Numerics;
using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Equatorial;
using ChargerAstronomyShared.Domain.Horizontal;
using ChargerAstronomyShared.Domain.Index;

namespace ChargerAstronomyShared.Domain.SpatialIndex
{
    public sealed class SpatialStarIndex 
    {
        public ITileIndex TileIndex { get; }
        public IReadOnlyList<HorizontalStar> Stars => stars;

        readonly List<HorizontalStar> stars;
        readonly Dictionary<TileId, List<HorizontalStar>> starsByTile = new Dictionary<TileId, List<HorizontalStar>>();

        public SpatialStarIndex(ITileIndex tileIndex)
            : this(tileIndex, Array.Empty<HorizontalStar>())
        {
        }

        public SpatialStarIndex(ITileIndex tileIndex, IEnumerable<HorizontalStar> inputStars)
        {
            TileIndex = tileIndex ?? throw new ArgumentNullException(nameof(tileIndex));
            stars = new List<HorizontalStar>(inputStars ?? throw new ArgumentNullException(nameof(inputStars)));

            foreach (var id in tileIndex.Enumerate())
                starsByTile[id] = new List<HorizontalStar>();

            foreach (var star in stars)
            {
                TileId tile = GetTileForStar(star);
                starsByTile[tile].Add(star);
            }
        }

        /// <summary>
        /// Adds a new star to the collection and associates it with the appropriate tile.
        /// </summary>
        /// <remarks>The star is added to the main collection and also indexed by its corresponding tile.
        /// If the tile does not already exist in the index, it will be created.</remarks>
        /// <param name="newStar">The star to add. Must not be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newStar"/> is <see langword="null"/>.</exception>
        public void AddStar(HorizontalStar newStar)
        {
            if (newStar == null)
                throw new ArgumentNullException(nameof(newStar));

            stars.Add(newStar);
            TileId tile = GetTileForStar(newStar);

            if (!starsByTile.ContainsKey(tile))
                starsByTile[tile] = new List<HorizontalStar>();

            starsByTile[tile].Add(newStar);
        }

        /// <summary>
        /// Adds a collection of stars to the collection and associates it with the appropriate tile.
        /// </summary>
        /// <remarks>Each star is added to the main collection and also indexed by its corresponding tile.
        /// If the tile does not already exist in the index, it will be created.</remarks>
        /// <param name="newStars">A <see cref="PageResult{HorizontalStar}"/> containing the stars to add.  The collection must not be <see
        /// langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newStars"/> is <see langword="null"/>.</exception>
        public void AddStar(PageResult<HorizontalStar> newStars)
        {
            if (newStars == null)
                throw new ArgumentNullException(nameof(newStars));

            foreach (var star in newStars.Items)
            {
                stars.Add(star);
                TileId tile = GetTileForStar(star);

                if (!starsByTile.ContainsKey(tile))
                    starsByTile[tile] = new List<HorizontalStar>();

                starsByTile[tile].Add(star);
            }
        }

        /// <summary>
        /// Returns the <see cref="TileId"/> containing the given star.
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        public TileId GetTileForStar(HorizontalStar star)
        {
            var dir = ToUnitVector(star);
            return TileIndex.DirectionToTileId(dir);
        }

        public IReadOnlyList<HorizontalStar> GetStarsInTile(TileId tile)
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
        static Vector3 ToUnitVector(HorizontalStar star)
        {
            double raRad = star.RightAscension * Math.PI / 180.0;
            double decRad = star.Declination * Math.PI / 180.0;

            float x = (float)(Math.Cos(decRad) * Math.Cos(raRad));
            float y = (float)(Math.Cos(decRad) * Math.Sin(raRad));
            float z = (float)(Math.Sin(decRad));

            return Vector3.Normalize(new Vector3(x, y, z));
        }
    }
}
