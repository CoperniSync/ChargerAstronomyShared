using ChargerAstronomyShared.Domain.Horizontal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{

    /// <summary>
    /// Interface for horizontal celestial bodies.
    /// </summary>
    public interface IHorizontal
    {
        public HorizontalBody HorizontalBody { get; }

        /// <summary>
        /// Sets the visibility state of the horizontal body.
        /// </summary>
        /// <param name="visible">Visibility boolean.</param>
        public void SetState(bool visible);

        /// <summary>
        /// Updates the position of the horizontal body.
        /// </summary>
        public void UpdatePosition();
    }
}
