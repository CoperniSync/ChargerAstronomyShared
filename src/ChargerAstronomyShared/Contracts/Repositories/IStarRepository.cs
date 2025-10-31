using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Contracts.Streaming;
    using ChargerAstronomyShared.Domain.Equatorial;
    using System.Threading;

    public interface IStarRepository
    {
        /// <summary>
        /// Not implemented.
        /// </summary>
        Task<EquatorialStar> GetStarByIdAsync(int id);

        /// <summary>
        /// Not implemented.
        /// </summary>
        Task<PageResult<EquatorialStar>> GetAllAsync(PageRequest page);

        /// <summary>
        /// Not implemented.
        /// </summary>
        Task<PageResult<EquatorialStar>> QueryBySkyRegionAsync(SkyRegion skyRegion, PageRequest page);

        /// <summary>
        /// Asynchronously produces pages of data and enqueues them into the specified queue.
        /// </summary>
        /// <remarks>This method processes the specified <paramref name="firstPage"/> and continues
        /// producing subsequent pages  based on the page's configuration. Pages are enqueued into the <paramref
        /// name="queue"/> as they are produced. The operation can be canceled by signaling the <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="queue">The queue into which the produced pages will be enqueued.</param>
        /// <param name="firstPage">The initial page request that specifies the starting point for producing pages.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        Task ProducePagesAsync(IInitializationQueue<PageResult<EquatorialStar>> queue, 
            PageRequest firstPage, CancellationToken cancellationToken = default);

    }
}
