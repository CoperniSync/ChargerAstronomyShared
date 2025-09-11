using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    using ChargerAstronomyShared.Domain.Equatorial;

    public interface IMessierRepository
    {
        /// <summary>
        /// Gets all Messier Objects from the repository
        /// </summary>
        /// <returns>A <c>yieldable</c> <see cref="IEnumerable{EquatorialMessierObject}"/> that can be lazily loaded when needed.</returns>
        public IEnumerable<EquatorialMessierObject> GetMessierObjects();
    }
}
