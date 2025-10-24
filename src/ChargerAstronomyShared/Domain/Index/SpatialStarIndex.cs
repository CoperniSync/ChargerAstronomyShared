using System;
using System.Collections.Generic;
using System.Numerics;
using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Equatorial;
using ChargerAstronomyShared.Domain.Index;

namespace ChargerAstronomyShared.Domain.SpatialIndex
{
    public sealed class SpatialStarIndex 
    {
        public ITileIndex TileIndex { get; }
        public IReadOnlyList<EquatorialStar> Stars => stars;

        readonly List<EquatorialStar> stars;
        readonly Dictionary<TileId, List<EquatorialStar>> starsByTile = new Dictionary<TileId, List<EquatorialStar>>();

        public SpatialStarIndex(ITileIndex tileIndex, IEnumerable<EquatorialStar> inputStars)
        {
            TileIndex = tileIndex ?? throw new ArgumentNullException(nameof(tileIndex));
            stars = new List<EquatorialStar>(inputStars ?? throw new ArgumentNullException(nameof(inputStars)));

            foreach (var id in tileIndex.Enumerate())
                starsByTile[id] = new List<EquatorialStar>();

            foreach (var star in stars)
            {
                TileId tile = GetTileForStar(star);
                starsByTile[tile].Add(star);
            }
        }


        /// <summary>
        /// Returns the <see cref="TileId"/> containing the given star.
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        public TileId GetTileForStar(EquatorialStar star)
        {
            var dir = ToUnitVector(star);
            return TileIndex.DirectionToTileId(dir);
        }

        public IReadOnlyList<EquatorialStar> GetStarsInTile(TileId tile)
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
        static Vector3 ToUnitVector(EquatorialStar star)
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
