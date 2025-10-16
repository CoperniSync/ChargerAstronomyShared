using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain  
{
    /// <summary>
    /// The location of an observer on (or near) the surface of the Earth.
    /// </summary>
    /// <remarks>
    /// This structure is passed to functions that calculate phenomena as observed
    /// from a particular place on the Earth.
    /// </remarks>
    public struct Observer
    {
        /// <summary>
        /// Geographic latitude in degrees north (positive) or south (negative) of the equator.
        /// </summary>
        public readonly double latitude;

        /// <summary>
        /// Geographic longitude in degrees east (positive) or west (negative) of the prime meridian at Greenwich, England.
        /// </summary>
        public readonly double longitude;

        /// <summary>
        /// The height above (positive) or below (negative) sea level, expressed in meters.
        /// </summary>
        public readonly double height;

        /// <summary>
        /// Creates an Observer object.
        /// </summary>
        /// <param name="latitude">Geographic latitude in degrees north (positive) or south (negative) of the equator.</param>
        /// <param name="longitude">Geographic longitude in degrees east (positive) or west (negative) of the prime meridian at Greenwich, England.</param>
        /// <param name="height">The height above (positive) or below (negative) sea level, expressed in meters.</param>
        public Observer(double latitude, double longitude, double height)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.height = height;
        }

        /// <summary>
        /// Converts an `Observer` to a string representation like `(N 26.728965, W 093.157562, 1234.567 m)`.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(latitude < 0.0 ? "S " : "N ");
            sb.Append(Math.Abs(latitude).ToString("00.000000"));
            sb.Append(", ");
            sb.Append(longitude < 0.0 ? "W " : "E ");
            sb.Append(Math.Abs(longitude).ToString("000.000000"));
            sb.Append(", ");
            sb.Append(height.ToString("0.000"));
            sb.Append(" m)");
            return sb.ToString();
        }
    }
}
