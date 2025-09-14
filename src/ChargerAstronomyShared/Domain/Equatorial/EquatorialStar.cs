using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain.Equatorial
{
    //using ChargerAstronomyShared.Domain.Common;

    public sealed class EquatorialStar : EquatorialCelestialBody
    {
        /// <summary>
        /// The unique ID in the Yale Star catalog
        /// </summary>
        public int StarId { get; set; }
        /// <summary>
        /// The unique ID in the Hipparcos catalog
        /// </summary>
        public int? HipparcosId { get; set; }
        /// <summary>
        /// The unique ID in the Henry Draper catalog
        /// </summary>
        public int? HenryDraperId { get; set; }
        /// <summary>
        /// The unique ID in the Harvard Revised catalog
        /// </summary>
        public int? HarvardRevisedId { get; set; }
        /// <summary>
        /// Unique ID in the Gleise catalog
        /// </summary>
        public string? GlieseId { get; set; }
        /// <summary>
        /// A combination of two star-naming systems - Bayer (Greek letter) and Flamsteed (number)
        /// </summary>
        public string? BayerFlamsteedDesignation { get; set; }
        /// <summary>
        /// The international Astronomical Union (IAU) or traditional historical name of the star
        /// </summary>
        public string? ProperName { get; set; }
        /// <summary>
        /// The actual brightness of the star
        /// </summary>
        public double AbsoluteMagnitude { get; set; }
        /// <summary>
        /// A value defined by the Harvard classification to categorize stars according to color, temperature and spectral lines
        /// </summary>
        public string? Spectrum { get; set; }
        /// <summary>
        /// Describes the color of a star based on its brightness according to Blue and Visual Green filters.
        /// </summary>
        public double? ColorIndex { get; set; }
        /// <summary>
        /// Parallax in milliarcseconds (mas).
        /// Convert to parsecs: distance_pc = 1000 / ParallaxMas
        /// </summary>
        public double? ParallaxMas { get; set; }
        /// <summary>
        /// Proper motion in Right Ascension, milliarcseconds per year.
        /// </summary>
        public double? ProperMotionRaMasPerYear { get; set; }
        /// <summary>
        /// Proper motion in Declination, milliarcseconds per year.
        /// </summary>
        public double? ProperMotionDecMasPerYear { get; set; }
    }
}
