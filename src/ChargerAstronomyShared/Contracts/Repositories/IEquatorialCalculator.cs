using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain;
using ChargerAstronomyShared.Domain.Heat;
using ChargerAstronomyShared.Domain.Horizontal;
using ChargerAstronomyShared.Domain.SpatialIndex;

namespace ChargerAstronomyShared.Contracts.Repositories
{

    /// <summary>
    /// Converts a <see cref="HorizontalBody"/> to a {T} object
    /// Author: Josh Johner
    /// Created: SPR 2025
    /// </summary>
    /// <typeparam name="T">The converted value of type {T} (defined by the front end)</typeparam>
    public  interface IEquatorialCalculator
    {

        /// <summary>
        /// Increments the time of the internal universal time used to perform calculations.
        /// </summary>
        /// <param name="seconds"></param>
        public void IncrementTime(float deltaTime);

        /// <summary>
        /// Calculates and updates the position of an <see cref="Observer"/>
        /// </summary>
        /// <param name="hoBody">The body to perform the calculation on.</param>
        public void UpdateLocation(Observer newLocation);

        /// <summary>
        /// Updates the internal universal time used for calculations.
        /// </summary>
        /// <param name="newTime"></param>
        public void UpdateTime(CalendarDateTime newTime);

        /// <summary>
        /// Updates the internal universal time and <see cref="Observer"/> location for calculations.
        /// </summary>
        /// <param name="newTime"></param>
        /// <param name="newLocation"></param>
        public void UpdateTimeAndLocation(CalendarDateTime newTime, Observer newLocation);
    }
}
