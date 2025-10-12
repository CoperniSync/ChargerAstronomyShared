using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Heat;
using ChargerAstronomyShared.Domain.Horizontal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Interfaces
{
    public interface IEquatorialCalculator
    {
        HeatService HeatService { get; set; }

        double Latitude { get; }

        double Longitude { get; }
    
        HorizontalMoon CreateMoon();

        HorizontalSun CreateSun();

        IEnumerable<HorizontalPlanet> CreatePlanets();

        void SetLocation(double latitude, double longitude);

        void UpdateHeatedTiles(CalendarDate dateTime);

        void UpdatePositionOf(HorizontalStar star, CalendarDate dateTime);
    }
}
