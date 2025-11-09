using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    /// <summary>
    /// A request for a page of data, used in star queues.
    /// </summary>
    public sealed class PageRequest
    {
        /// <summary>
        /// The amount of total items to skip.
        /// </summary>
        public int Skip { get; }

        /// <summary>
        /// The amount of items to take, determines the quantity of items in the page.
        /// </summary>
        public int Take { get; }

        /// <summary>
        /// Initializes a new page request with the specified skip and take values.
        /// </summary>
        /// <remarks>Negatives values for <paramref name="skip"/> or <paramref
        /// name="take"/> will be replaced by 0.</remarks>
        /// <param name="skip">The number of items to skip. Must be greater than or equal to 0. Defaults to 0.</param>
        /// <param name="take">The number of items to take. Must be greater than or equal to 0. Defaults to 1000.</param>
        public PageRequest(int skip = 0, int take = 1000)
        {
            // Reasonable default to 1000

            Skip = skip < 0 ? 0 : skip;
            Take = take < 0 ? 0 : take;
        }

    }
}
