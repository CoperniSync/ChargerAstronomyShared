using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public sealed class PageResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int Total {  get; }
        public PageResult(IReadOnlyList<T> items, int total)
        {
            Items = items; Total = total;
        }
    }
}
