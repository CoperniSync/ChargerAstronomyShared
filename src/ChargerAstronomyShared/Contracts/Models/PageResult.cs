using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public sealed class PageResult<T>
    {
        public IReadOnlyList<T> Items { get; }

        public int Total { get; }

        public int Page { get; }

        public int Size { get; }

        public bool HasNext => Page * Size < Total;

        public PageResult(IReadOnlyList<T> items, int total, int page, int size)
        {
            Items = items; Total = total; Page = page; Size = size; 
        }
    }
}
