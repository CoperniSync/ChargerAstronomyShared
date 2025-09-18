
using ChargerAstronomyShared.Domain.Equatorial;

namespace ChargerAstronomyShared.Domain.Horizontal
{
    public abstract class HorizontalBody
    {
        /// <summary>
        /// The angle in decimal degrees formed between the horizon and the star
        /// </summary>
        public double Altitude { get; internal set; }
        /// <summary>
        /// The angle in decimal degrees formed between due north and the star
        /// </summary>
        public double Azimuth { get; internal set; }
    }
}