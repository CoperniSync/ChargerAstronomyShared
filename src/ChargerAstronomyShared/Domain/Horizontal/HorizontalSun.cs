using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChargerAstronomyShared.Domain.Equatorial;

namespace ChargerAstronomyShared.Domain.HorizontalObjects
{
    /// <summary>
    /// Represents the Sun in horizontal coordinate form.
    /// </summary>
    public class HorizontalSun : HorizontalBody
    {
        /// <summary>
        /// Creates a new instance using a celestial body to contain basic information
        /// </summary>
        /// <param name="body">A new <c>HorizontalSun</c> object.</param>
        public HorizontalSun(EquatorialCelestialBody body) : base(body)
        {
            // empty in old stargazer - jojo
        }
    }
}
