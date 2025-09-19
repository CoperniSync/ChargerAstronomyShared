using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public sealed class PageResult<T>
    {
        public IReadOnlyList<T> Items { get; }

        public int Total { get; }

        public int Skip { get; }

        public int Take { get; }

        public bool HasNext => Skip + Items.Count < Total;

        public PageResult(IReadOnlyList<T> items, int total, int skip, int take)
        {
            Items = items; Total = total; Skip = skip; Take = take; 
        }

        public PageRequest NextRequest() => new PageRequest(Skip + Items.Count, Take);
    }
}
