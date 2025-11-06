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

        public IReadOnlyList<HorizontalStar> Stars => stars;
        readonly List<HorizontalStar> stars;

        /// <summary>
        /// The index of the tile associated with this instance.
        /// </summary>
        public ITileIndex TileIndex { get; }

        /// <summary>
        /// A dictionary mapping <see cref="TileId"/> to a list of <see cref="HorizontalStar"/> objects contained within that tile.
        /// </summary>
        readonly Dictionary<TileId, List<HorizontalStar>> starsByTile = new Dictionary<TileId, List<HorizontalStar>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialStarIndex"/> class using the specified tile index.
        /// </summary>
        /// <param name="tileIndex">The tile index used to organize and manage spatial data.</param>
        public SpatialStarIndex(ITileIndex tileIndex)
            : this(tileIndex, Array.Empty<HorizontalStar>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialStarIndex"/> class, organizing stars into tiles based on
        /// their spatial positions.
        /// </summary>
        /// <param name="tileIndex">The tile index used to define the spatial tiling structure. Cannot be <see langword="null"/>.</param>
        /// <param name="inputStars">The collection of stars to be indexed. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="tileIndex"/> or <paramref name="inputStars"/> is <see langword="null"/>.</exception>
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
        /// <param name="star">The given <see cref="HorizontalStar"/>.</param>
        /// <returns>The tile that contains the given star.</returns>
        public TileId GetTileForStar(HorizontalStar star)
        {
            var dir = ToUnitVector(star);
            return TileIndex.DirectionToTileId(dir);
        }

        /// <summary>
        /// Retrieves the list of stars located within the specified tile.
        /// </summary>
        /// <param name="tile">The identifier of the tile for which to retrieve the stars.</param>
        /// <returns>A read-only list of <see cref="HorizontalStar"/> within the specified tile.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <paramref name="tile"/> does not exist in the collection.</exception>
        public IReadOnlyList<HorizontalStar> GetStarsInTile(TileId tile)
        {
            if (!starsByTile.TryGetValue(tile, out var list))
                throw new ArgumentOutOfRangeException(nameof(tile));

            return list;
        }

        /// <summary>
        /// Converts the equatorial coordinates of a star to a unit vector.
        /// </summary>
        /// <param name="star">The given <see cref="HorizontalStar".</param>
        /// <returns>A 3rd degree vector pointing to the given star.</returns>
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
