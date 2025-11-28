using ChargerAstronomyShared.Domain.Horizontal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public interface IHorizontal
    {
        public HorizontalBody HorizontalBody { get; }

        public void SetState(bool visible);

        public void UpdatePosition();
    }
}
