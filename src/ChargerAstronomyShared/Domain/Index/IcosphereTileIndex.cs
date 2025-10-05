using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Net;
using System.Runtime.InteropServices;

namespace ChargerAstronomyShared.Domain.Index
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Geometry;

    public sealed class IcosphereTileIndex : ITileIndex
    {
        public int TileCount => tiles.Count;

        public IReadOnlyList<TileId> Tiles => tiles.AsReadOnly();

        public IcosphereTileIndex(int subdivisions = 0)
        {
            if (subdivisions < 0 || subdivisions > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(subdivisions), "Subdivisions must be between 0 and 5");
            }

            vertices.AddRange(GetIsocahedronVertices());
            faces.AddRange(GetIsocahedronIndices());

            for (int i = 0; i < subdivisions; i++)
            {
                SubdivideIcosphere();
            }

            // Normalize vertices to lie on the unit sphere (not really important for the spatial index
            // but we may want to visualize the spatial index later in unity so go ahead and do this)
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = Vector3.Normalize(vertices[i]);
            }

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                var v1 = vertices[face.a - 1]; // -1 because our indices are 1-based
                var v2 = vertices[face.b - 1];
                var v3 = vertices[face.c - 1];

                var center = Vector3.Normalize((v1 + v2 + v3) / 3);
                var alpha = Math.Acos(Vector3.Dot(center, v2)); 

                var tileId = new TileId(i);
                tiles.Add(tileId);
                tileGeometryMap[tileId] = new TileGeometry(tileId, center, alpha, new List<Vector3> { v1, v2, v3 });
            }
        }

        public TileId DirectionToTileId(Vector3 direction)
        {
            const float EPS = 1e-7f;

            foreach ((TileId tileId, TileGeometry tileGeometry) in EnumerateGeometry())
            {
                var v1 = tileGeometry.Vertices[0];
                var v2 = tileGeometry.Vertices[1];
                var v3 = tileGeometry.Vertices[2];
                
                var n = tileGeometry.Center;

                // from (0 + t * d - p0) . n = 0

                float denom = Vector3.Dot(n, direction);
                if (denom <= EPS) continue; 

                float t = Vector3.Dot(n, v1) / denom;
                var p = direction * t;

                // cramer's rule wont work for points in worldspace
                // so we form a basis with u,v on the plane of the triangle and n normal to it

                float dot = MathF.Abs(Vector3.Dot(n, Vector3.UnitY));
                Vector3 helper = dot < 1 - EPS ? Vector3.UnitY : Vector3.UnitZ;

                var u = Vector3.Normalize(Vector3.Cross(helper, n));
                var v = Vector3.Cross(n, u);

                v1 = new Vector3(Vector3.Dot(v1, u), Vector3.Dot(v1, v), 0);
                v2 = new Vector3(Vector3.Dot(v2, u), Vector3.Dot(v2, v), 0);
                v3 = new Vector3(Vector3.Dot(v3, u), Vector3.Dot(v3, v), 0);
                p = new Vector3(Vector3.Dot(p, u), Vector3.Dot(p, v), 0);

                // just cramer's rule

                float D = (v1.X - v3.X) * (v2.Y - v3.Y) - (v2.X - v3.X) * (v1.Y - v3.Y);
                if (MathF.Abs(D) < EPS)
                    continue; 

                float a = ((p.X - v3.X) * (v2.Y - v3.Y) - (v2.X - v3.X) * (p.Y - v3.Y)) / D;
                float b = ((v1.X - v3.X) * (p.Y - v3.Y) - (p.X - v3.X) * (v1.Y - v3.Y)) / D;
                float c = 1f - a - b;

                // using barycentric coordinates we know if a,b,c > 0 
                // then our point lies within the triangle 

                if (a >= -EPS && b >= -EPS && c >= -EPS)
                    return tileId;
            }

            // This should never happen unless the sphere is not fully covered by tiles
            throw new InvalidOperationException($"Tile for direction {direction.ToString()} not found");
        }

        public IEnumerable<TileId> Neigbors(TileId id)
        {
            // This will be a pretty difficult operation to implement, going to skip for now.
            throw new NotImplementedException();
        }

        public IEnumerable<TileId> Enumerate()
        {
            foreach( TileId tileId in tiles)
            {
                yield return tileId;
            }
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
            if(tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry.Center;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }

        public TileGeometry GetGeometry(TileId id)
        {
            if(tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }

        private void SubdivideIcosphere()
        {
            var newFaces = new List<(int a, int b, int c)>(faces.Count * 4);

            // cache midpoints so we dont create two midpoints for the same edge
            var midpointCache = new Dictionary<(int, int), int>(faces.Count * 3);

            int Midpoint(int i0, int i1)
            {
                var key = i0 < i1 ? (i0, i1) : (i1, i0);
                if (midpointCache.TryGetValue(key, out int idx))
                    return idx;

                var m = (vertices[i0] + vertices[i1]) * 0.5f;
                idx = vertices.Count;
                vertices.Add(m);
                midpointCache[key] = idx;
                return idx;
            }

            int faceCount = faces.Count; 
            for (int i = 0; i < faceCount; i++)
            {
                var (a, b, c) = faces[i];

                int mid12 = Midpoint(a, b); // between a-b
                int mid23 = Midpoint(b, c); // between b-c
                int mid31 = Midpoint(c, a); // between c-a

                // (a -> b -> c)
                newFaces.Add((a, mid12, mid31));
                newFaces.Add((b, mid23, mid12));
                newFaces.Add((c, mid31, mid23));
                newFaces.Add((mid12, mid23, mid31));
            }

            faces.Clear();
            faces.AddRange(newFaces);
        }


        private static List<Vector3> GetIsocahedronVertices()
        {
            //https://en.wikipedia.org/wiki/Regular_icosahedron#Construction
            // (0, +-1, +-phi), (+-1, +-phi, 0), (+-phi, 0, +-1)

            float phi = (1 + (float)Math.Sqrt(5)) / 2; // golden ratio
            return new List<Vector3>()
            {
                // (+-1, +-phi, 0)
                new Vector3(-1,  phi, 0),
                new Vector3( 1,  phi, 0),
                new Vector3(-1, -phi, 0),
                new Vector3( 1, -phi, 0),

                // (0, +-1, +-phi)
                new Vector3(0, -1, phi),
                new Vector3(0, 1,  phi),
                new Vector3(0, -1, -phi),
                new Vector3(0, 1, -phi),

                // (+-phi, 0, +-1)
                new Vector3( phi, 0, -1),
                new Vector3( phi, 0,  1),
                new Vector3(-phi, 0, -1),
                new Vector3(-phi, 0,  1),
            };
        }

        private static List<(int a, int b, int c)> GetIsocahedronIndices()
        {
            // This is built with the vertices in the order given by GetIsocahedronVertices
            // So if you change the ordering of the vertices above this will fail. 

            return new List<(int a, int b, int c)>()
            {
                (1, 12, 6), (1, 6 , 2), (1, 2, 8), (1, 8, 11), (1, 11, 12),
                (2, 6, 10), (6, 12, 5), (12, 11, 3), (11, 8, 7), (8, 2, 9),
                (4, 10, 5), (4, 5, 3), (4, 3, 7), (4, 7, 9), (4, 9, 10),
                (5, 10, 6), (3, 5, 12), (7, 3, 11), (9, 7, 8), (10, 9, 2)
            };
        }

        private readonly List<TileId> tiles = new List<TileId>();
        private readonly Dictionary<TileId, TileGeometry> tileGeometryMap = new Dictionary<TileId, TileGeometry>();
        private readonly List<Vector3> vertices = new List<Vector3>();
        private readonly List<(int a, int b, int c)> faces = new List<(int a, int b, int c)>();

    }
}
