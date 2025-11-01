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
        public void IncrementTime(float deltaTime);
        public void UpdateLocation(Observer newLocation);
        public void UpdateTime(CalendarDateTime newTime);
        public void UpdateTimeAndLocation(CalendarDateTime newTime, Observer newLocation);
    }
}

