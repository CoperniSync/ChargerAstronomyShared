using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    /// <summary>
    /// The response for a page request, used in star queues.
    /// </summary>
    public sealed class PageResult<T>
    {
        /// <summary>
        /// Gets the collection of items in the current list.
        /// </summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>
        /// Total amount of items available.
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// The amount of items to skip.
        /// </summary>
        public int Skip { get; }

        /// <summary>
        /// The amount of items to take, determines the quantity of items in the page.
        /// </summary>
        public int Take { get; }

        /// <summary>
        /// Boolean indicating if there is a next page available.
        /// </summary>
        public bool HasNext => Skip + Items.Count < Total;

        /// <summary>
        /// Initializes a new page result with the specified items, total, skip, and take values.
        /// </summary>
        /// <param name="items">A read-only list of all items.</param>
        /// <param name="total">The total amount of items.</param>
        /// <param name="skip">The amount of items to skip.</param>
        /// <param name="take">The amount of items to take.</param>
        public PageResult(IReadOnlyList<T> items, int total, int skip, int take)
        {
            Items = items; Total = total; Skip = skip; Take = take; 
        }

        /// <summary>
        /// Recursively gets the next page request.
        /// </summary>
        /// <returns>A new PageRequest containing the next page.</returns>
        public PageRequest NextRequest() => new PageRequest(Skip + Items.Count, Take);
    }
}
