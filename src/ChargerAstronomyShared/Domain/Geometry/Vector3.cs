using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain.Geometry
{
    public readonly struct Vector3
    {
        public readonly double X, Y, Z;
        public Vector3(double x, double y, double z) { X=x; Y=y; Z=z; }
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X+b.X, a.Y+b.Y, a.Z+b.Z);
        public static Vector3 operator *(Vector3 a, double s) => new Vector3(a.X*s, a.Y*s, a.Z*s);
        public static double Dot(in Vector3 a, in Vector3 b) => a.X*b.X+a.Y*b.Y+a.Z*b.Z;
        public static double Len(in Vector3 a) => Math.Sqrt(Dot(a, a));
        public static Vector3 Normalize(in Vector3 a) { var l = Len(a); return new Vector3(a.X/l, a.Y/l, a.Z/l); }
        public static double Angle(in Vector3 a, in Vector3 b)
            { var d = Dot(a, b); d=Math.Clamp(d, -1.0, 1.0); return Math.Acos(d); }
    }
}
