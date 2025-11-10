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

        /// <inheritdoc/>
        public int TileCount => tiles.Count;

        public IReadOnlyList<TileId> Tiles => tiles.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="IcosphereTileIndex"/> class with a specified number of
        /// subdivisions.
        /// </summary>
        /// <remarks>This constructor generates an icosphere by subdividing a base icosahedron the
        /// specified number of times.  Each vertex of the resulting icosphere is normalized to lie on the unit sphere. 
        /// The icosphere is then used to initialize a spatial index, where each triangular face of the icosphere  is
        /// associated with a unique tile identifier and its corresponding geometry.</remarks>
        /// <param name="subdivisions">The number of times the base icosahedron is subdivided to create the icosphere.  Must be between 0 and 5,
        /// inclusive. Higher values result in a finer-grained icosphere.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="subdivisions"/> is less than 0 or greater than 5.</exception>
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
                var v1 = vertices[face.a]; // -1 because our indices are 1-based
                var v2 = vertices[face.b];
                var v3 = vertices[face.c];

                var center = Vector3.Normalize((v1 + v2 + v3) / 3);
                var alpha = Math.Acos(Vector3.Dot(center, v2)); 

                var tileId = new TileId(i);
                tiles.Add(tileId);
                tileGeometryMap[tileId] = new TileGeometry(tileId, center, alpha, new List<Vector3> { v1, v2, v3 });
            }
        }

        /// <inheritdoc/>
        public TileId DirectionToTileId(Vector3 direction)
        {
            const float EPS = 1e-7f;

            foreach ((TileId tileId, TileGeometry tileGeometry) in EnumerateGeometry())
            {
                var v1 = tileGeometry.Vertices[0];
                var v2 = tileGeometry.Vertices[1];
                var v3 = tileGeometry.Vertices[2];
                
                var n = Vector3.Normalize(Vector3.Cross(v2 - v1, v3 - v1));
                
                if (Vector3.Dot(n, tileGeometry.Center) < 0) n = -n;
                
                float d = Vector3.Dot(n, v1);
                
                float denom = Vector3.Dot(n, direction);
                if (denom <= EPS) continue;
                
                float t = d / denom;
                if (t <= EPS) continue;

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

        /// <inheritdoc/>
        public IEnumerable<TileId> Neigbors(TileId id)
        {
            // This will be a pretty difficult operation to implement, going to skip for now.
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<TileId> Enumerate()
        {
            foreach( TileId tileId in tiles)
            {
                yield return tileId;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Tuple<TileId, TileGeometry>> EnumerateGeometry()
        {
            foreach (var kvp in tileGeometryMap)
            {
                yield return Tuple.Create(kvp.Key, kvp.Value);
            }
        }

        /// <inheritdoc/>
        public double GetTileAlpha(TileId id)
        {
            if (tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry.Alpha;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }

        /// <inheritdoc/>
        public Vector3 GetTileCenter(TileId id)
        {
            if(tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry.Center;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }

        /// <inheritdoc/>
        public TileGeometry GetGeometry(TileId id)
        {
            if(tileGeometryMap.TryGetValue(id, out var geometry))
            {
                return geometry;
            }
            throw new Exception($"TileGeometry for TileID {id.Index} not found");
        }

        /// <summary>
        /// Subdivides the faces of the icosphere, increasing its resolution by adding new vertices and faces.
        /// </summary>
        /// <remarks>This method refines the current icosphere by splitting each triangular face into four
        /// smaller triangles. New vertices are added at the midpoints of the edges of the existing faces. The method
        /// modifies the <c>vertices</c> and <c>faces</c> collections to reflect the updated geometry.</remarks>
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

        /// <summary>
        /// Generates the vertices of a regular icosahedron centered at the origin.
        /// </summary>
        /// <remarks>The vertices are calculated based on the mathematical construction of a regular
        /// icosahedron, using the golden ratio (φ). The resulting vertices are returned as a list of <see
        /// cref="Vector3"/>  objects, where each vertex is represented by its 3D coordinates.</remarks>
        /// <returns>A <see cref="List{Vector3}"/> containing the 12 vertices of the icosahedron.</returns>
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

        /// <summary>
        /// Generates a list of triangular face indices representing the faces of an icosahedron.
        /// </summary>
        /// <remarks>Each tuple in the returned list represents a triangular face, where the integers
        /// correspond to the indices of the vertices that form the triangle. The vertex indices are expected
        /// to align with the ordering of vertices provided by the <c>GetIsocahedronVertices</c> method.       
        /// Modifying the vertex ordering may result in incorrect face definitions.</remarks>
        /// <returns>A list of tuples, where each tuple contains three integers representing the indices of the vertices that
        /// form a triangular face of the icosahedron.</returns>
        private static List<(int a, int b, int c)> GetIsocahedronIndices()
        {
            // This is built with the vertices in the order given by GetIsocahedronVertices
            // So if you change the ordering of the vertices above this will fail. 

            return new List<(int a, int b, int c)>()
            {
                (0,11,5), (0,5,1), (0,1,7), (0,7,10), (0,10,11),
                (1,5,9),  (5,11,4), (11,10,2), (10,7,6), (7,1,8),
                (3,9,4),  (3,4,2),  (3,2,6),  (3,6,8),  (3,8,9),
                (4,9,5),  (2,4,11), (6,2,10), (8,6,7),  (9,8,1)
            };
        }

        private readonly List<TileId> tiles = new List<TileId>();
        private readonly Dictionary<TileId, TileGeometry> tileGeometryMap = new Dictionary<TileId, TileGeometry>();
        private readonly List<Vector3> vertices = new List<Vector3>();
        private readonly List<(int a, int b, int c)> faces = new List<(int a, int b, int c)>();

    }
}
