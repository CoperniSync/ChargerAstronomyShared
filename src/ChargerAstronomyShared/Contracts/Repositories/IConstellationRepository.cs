using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    using ChargerAstronomyShared.Domain.Equatorial;

    public interface IConstellationRepository
    {
        /// <summary>
        /// Gets all <see cref="Constellation"/>s from the repository.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Constellation}"/> containing graphs of all the constellations.</returns>
        public IEnumerable<Constellation> GetConstellations();
    }
}
