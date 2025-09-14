using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Equatorial;

    public interface IStarRepository
    {
        Task<EquatorialStar> GetStarByIdAsync(int id);

        Task<PageResult<EquatorialStar>> GetAllAsync(PageRequest page);

        Task<PageResult<EquatorialStar>> QueryBySkyRegionAsync(SkyRegion skyRegion, PageRequest page);

        // Possibly query by icosphere region
        // Possibly query by proper name
        // Maybe add some filtering capabilities

    }
}
