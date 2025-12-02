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
        /// <param name="deltaTime">The amount of time to increase by, in seconds.</param>
        public void IncrementTime(float deltaTime);

        /// <summary>
        /// Calculates and updates the position of an <see cref="Observer"/>
        /// </summary>
        /// <param name="newLocation">The new location for the <see cref="Observer"/> to update to.</param>
        public void UpdateLocation(Observer newLocation);

        /// <summary>
        /// Updates the internal universal time used for calculations.
        /// </summary>
        /// <param name="newTime">The new universal time.</param>
        public void UpdateTime(CalendarDateTime newTime);

        /// <summary>
        /// Updates the internal universal time and <see cref="Observer"/> location for calculations.
        /// </summary>
        /// <param name="newTime">The new universal time.</param>
        /// <param name="newLocation">The new location for the <see cref="Observer"/> to update to.</param>
        public void UpdateTimeAndLocation(CalendarDateTime newTime, Observer newLocation);

        /// <summary>
        /// Updates the position of a planet based on the current time and <see cref="Observer"/>.
        /// </summary>
        /// <param name="moon">A planet.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="planet"/> name is not recognized in the internal collection of known planets.</exception>
        public void UpdatePositionOf(HorizontalPlanet planet);

        /// <summary>
        /// Updates the position of the moon based on the current time and <see cref="Observer"/>.
        /// </summary>
        /// <param name="moon">The Moon.</param>
        public void UpdatePositionOf(HorizontalMoon moon);

        /// <summary>
        /// Updates the position of the Sun based on the current time and <see cref="Observer"/>.
        /// </summary>
        /// <param name="moon">The Sun.</param>
        public void UpdatePositionOf(HorizontalSun sun);
    }
}
