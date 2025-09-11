using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public sealed class PageRequest
    {
        public int Skip {  get; }
        public int Take {  get; }
        public PageRequest(int skip=0, int take=1000)
        {
            // Reasonable default to 1000

            Skip = skip < 0 ? 0 : skip;
            Take = take < 0 ? 0 : take;
        }
    } 
}
