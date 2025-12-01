using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Index
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Geometry;

    public sealed class CubeMapTileIndex : ITileIndex
    {
        private readonly int subdivisionsPerFace;
        private readonly List<TileId> tiles = new List<TileId>();
        private readonly Dictionary<TileId, TileGeometry> tileGeometryMap = new Dictionary<TileId, TileGeometry>();

        public int TileCount => tiles.Count;

        public IReadOnlyList<TileId> Tiles => tiles.AsReadOnly();

        private enum CubeFace
        {
            PositiveX = 0,
            NegativeX = 1,
            PositiveY = 2,
            NegativeY = 3,
            PositiveZ = 4,
            NegativeZ = 5
        }

        public CubeMapTileIndex(int subdivisionsPerFace = 4)
        {
            if (subdivisionsPerFace < 1)
                throw new ArgumentOutOfRangeException(nameof(subdivisionsPerFace), "Must have at least 1 subdivision per face");

            this.subdivisionsPerFace = subdivisionsPerFace;
            BuildTiles();
        }

        private void BuildTiles()
        {
            int tileIndex = 0;

            for (int faceIdx = 0; faceIdx < 6; faceIdx++)
            {
                var face = (CubeFace)faceIdx;

                for (int v = 0; v < subdivisionsPerFace; v++)
                {
                    for (int u = 0; u < subdivisionsPerFace; u++)
                    {
                        float uMin = -1f + (2f * u / subdivisionsPerFace);
                        float uMax = -1f + (2f * (u + 1) / subdivisionsPerFace);
                        float vMin = -1f + (2f * v / subdivisionsPerFace);
                        float vMax = -1f + (2f * (v + 1) / subdivisionsPerFace);

                        float uCen = (uMin + uMax) / 2f;
                        float vCen = (vMin + vMax) / 2f;

                        Vector3 center = Vector3.Normalize(CubeFaceUVToVector(face, uCen, vCen));

                        var vertices = new List<Vector3>
                        {
                            Vector3.Normalize(CubeFaceUVToVector(face, uMin, vMin)),
                            Vector3.Normalize(CubeFaceUVToVector(face, uMax, vMin)),
                            Vector3.Normalize(CubeFaceUVToVector(face, uMax, vMax)),
                            Vector3.Normalize(CubeFaceUVToVector(face, uMin, vMax))
                        };

                        double alpha = CalculateTileAlpha(center, vertices);

                        var tileId = new TileId(tileIndex++);
                        tiles.Add(tileId);
                        tileGeometryMap[tileId] = new TileGeometry(tileId, center, alpha, vertices);
                    }
                }
            }
        }

        private static Vector3 CubeFaceUVToVector(CubeFace face, float u, float v)
        {
            return face switch
            {
                CubeFace.PositiveX => new Vector3(1f, u, v),    // +x face: varies in y and z
                CubeFace.NegativeX => new Vector3(-1f, -u, v),  // -x face: mirror in y
                CubeFace.PositiveY => new Vector3(-u, 1f, v),   // +y face: varies in x and z
                CubeFace.NegativeY => new Vector3(u, -1f, v),   // -y face: varies in x and z
                CubeFace.PositiveZ => new Vector3(u, v, 1f),    // +z face: varies in x and y
                CubeFace.NegativeZ => new Vector3(-u, v, -1f),  // -z face: mirror in X
                _ => throw new ArgumentException("Invalid cube face")
            };
        }

        private static (CubeFace face, float u, float v) VectorToCubeFaceUV(Vector3 direction)
        {
            float absX = Math.Abs(direction.X);
            float absY = Math.Abs(direction.Y);
            float absZ = Math.Abs(direction.Z);

            CubeFace face;
            float u, v;

            // axis check
            if (absX >= absY && absX >= absZ)
            {
                // x
                if (direction.X > 0)
                {
                    face = CubeFace.PositiveX;
                    u = direction.Y / absX;
                    v = direction.Z / absX;
                }
                else
                {
                    face = CubeFace.NegativeX;
                    u = -direction.Y / absX;
                    v = direction.Z / absX;
                }
            }
            else if (absY >= absZ)
            {
                // y 
                if (direction.Y > 0)
                {
                    face = CubeFace.PositiveY;
                    u = -direction.X / absY;
                    v = direction.Z / absY;
                }
                else
                {
                    face = CubeFace.NegativeY;
                    u = direction.X / absY;
                    v = direction.Z / absY;
                }
            }
            else
            {
                // z 
                if (direction.Z > 0)
                {
                    face = CubeFace.PositiveZ;
                    u = direction.X / absZ;
                    v = direction.Y / absZ;
                }
                else
                {
                    face = CubeFace.NegativeZ;
                    u = -direction.X / absZ;
                    v = direction.Y / absZ;
                }
            }

            return (face, u, v);
        }

        private static double CalculateTileAlpha(Vector3 center, List<Vector3> vertices)
        {
            double maxAngle = 0.0;
            foreach (var vertex in vertices)
            {
                float dot = Vector3.Dot(center, vertex);
                dot = Math.Clamp(dot, -1f, 1f);
                double angle = Math.Acos(dot);
                maxAngle = Math.Max(maxAngle, angle);
            }
            return maxAngle;
        }

        public TileId DirectionToTileId(Vector3 direction)
        {
            direction = Vector3.Normalize(direction);

            var (face, u, v) = VectorToCubeFaceUV(direction);

            int uIdx = (int)((u + 1f) / 2f * subdivisionsPerFace);
            int vIdx = (int)((v + 1f) / 2f * subdivisionsPerFace);

            uIdx = Math.Clamp(uIdx, 0, subdivisionsPerFace - 1);
            vIdx = Math.Clamp(vIdx, 0, subdivisionsPerFace - 1);

            int tilesPerFace = subdivisionsPerFace * subdivisionsPerFace;
            int tileIndex = (int)face * tilesPerFace + vIdx * subdivisionsPerFace + uIdx;

            return tiles[tileIndex];
        }

        /// <inheritdoc/>
        public IEnumerable<TileId> Neigbors(TileId id)
        {
            throw new NotImplementedException();
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