using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Index
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Geometry;

    public sealed class UVSphereTileIndex : ITileIndex
    {
        private readonly int raSteps;
        private readonly int decSteps;
        private readonly List<TileId> tiles = new List<TileId>();
        private readonly Dictionary<TileId, TileGeometry> tileGeometryMap = new Dictionary<TileId, TileGeometry>();

        public int TileCount => tiles.Count;

        public IReadOnlyList<TileId> Tiles => tiles.AsReadOnly();

        public UVSphereTileIndex(int raSteps = 24, int decSteps = 18)
        {
            if (raSteps < 2)
                throw new ArgumentOutOfRangeException(nameof(raSteps), "Must have at least 2 RA steps");
            if (decSteps < 2)
                throw new ArgumentOutOfRangeException(nameof(decSteps), "Must have at least 2 Dec steps");

            this.raSteps = raSteps;
            this.decSteps = decSteps;

            BuildTiles();
        }

        private void BuildTiles()
        {
            double raStep = 2.0 * Math.PI / raSteps; 
            double decStep = Math.PI / decSteps;     

            int tileIndex = 0;

            for (int decIdx = 0; decIdx < decSteps; decIdx++)
            {
                for (int raIdx = 0; raIdx < raSteps; raIdx++)
                {
                    double raMin = raIdx * raStep;
                    double raMax = (raIdx + 1) * raStep;
                    double decMin = -Math.PI / 2.0 + decIdx * decStep;
                    double decMax = -Math.PI / 2.0 + (decIdx + 1) * decStep;

                    double raCen = (raMin + raMax) / 2.0;
                    double decCen = (decMin + decMax) / 2.0;

                    Vector3 center = SphericalToCartesian(raCen, decCen);

                    // based on furthest corner from center
                    double alpha = CalculateTileAlpha(raMin, raMax, decMin, decMax, center);

                    // generate corner vertices for visualization
                    var vertices = new List<Vector3>
                    {
                        SphericalToCartesian(raMin, decMin),
                        SphericalToCartesian(raMax, decMin),
                        SphericalToCartesian(raMax, decMax),
                        SphericalToCartesian(raMin, decMax)
                    };

                    var tileId = new TileId(tileIndex++);
                    tiles.Add(tileId);
                    tileGeometryMap[tileId] = new TileGeometry(tileId, center, alpha, vertices);
                }
            }
        }

        private static Vector3 SphericalToCartesian(double ra, double dec)
        {
            float x = (float)(Math.Cos(dec) * Math.Cos(ra));
            float y = (float)(Math.Cos(dec) * Math.Sin(ra));
            float z = (float)(Math.Sin(dec));
            return new Vector3(x, y, z);
        }


        private static double CalculateTileAlpha(double raMin, double raMax, double decMin, double decMax, Vector3 center)
        {
            var corners = new[]
            {
                SphericalToCartesian(raMin, decMin),
                SphericalToCartesian(raMax, decMin),
                SphericalToCartesian(raMax, decMax),
                SphericalToCartesian(raMin, decMax)
            };

            double maxAngle = 0.0;
            foreach (var corner in corners)
            {
                float dot = Vector3.Dot(center, corner);
                dot = Math.Clamp(dot, -1f, 1f);
                double angle = Math.Acos(dot);
                maxAngle = Math.Max(maxAngle, angle);
            }

            return maxAngle;
        }

        public TileId DirectionToTileId(Vector3 direction)
        {
            direction = Vector3.Normalize(direction);

            double dec = Math.Asin(direction.Z);
            double ra = Math.Atan2(direction.Y, direction.X);

            if (ra < 0) ra += 2.0 * Math.PI;

            double raStep = 2.0 * Math.PI / raSteps;
            double decStep = Math.PI / decSteps;

            int raIdx = (int)(ra / raStep);
            int decIdx = (int)((dec + Math.PI / 2.0) / decStep);

            raIdx = Math.Clamp(raIdx, 0, raSteps - 1);
            decIdx = Math.Clamp(decIdx, 0, decSteps - 1);

            int tileIndex = decIdx * raSteps + raIdx;
            return tiles[tileIndex];
        }

        public IEnumerable<TileId> Neigbors(TileId id)
        {
            int index = id.Index;
            int raIdx = index % raSteps;
            int decIdx = index / raSteps;

            var neighbors = new List<TileId>();

            for (int dDec = -1; dDec <= 1; dDec++)
            {
                for (int dRA = -1; dRA <= 1; dRA++)
                {
                    if (dDec == 0 && dRA == 0) continue; 

                    int nDecIdx = decIdx + dDec;
                    int nRAIdx = raIdx + dRA;

                    if (nDecIdx < 0 || nDecIdx >= decSteps) continue;

                    if (nRAIdx < 0) nRAIdx += raSteps;
                    if (nRAIdx >= raSteps) nRAIdx -= raSteps;

                    int neighborIndex = nDecIdx * raSteps + nRAIdx;
                    neighbors.Add(tiles[neighborIndex]);
                }
            }

            return neighbors;
        }

        public IEnumerable<TileId> Enumerate()
        {
            return tiles;
        }

        public IEnumerable<Tuple<TileId, TileGeometry>> EnumerateGeometry()
        {
            foreach (var kvp in tileGeometryMap)
            {
                yield return Tuple.Create(kvp.Key, kvp.Value);
            }
        }

        public double GetTileAlpha(TileId id)
        {
            if (tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry.Alpha;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }

        public Vector3 GetTileCenter(TileId id)
        {
            if (tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry.Center;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }

        public TileGeometry GetGeometry(TileId id)
        {
            if (tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }
    }
}