using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    // From cosine kitty astronomy class

    public struct CalendarDate
    {
        /// <summary>The year value in the range -999999 to +999999.</summary>
        public int year;

        /// <summary>The calendar month in the range 1..12.</summary>
        public int month;

        /// <summary>The day of the month in the reange 1..31.</summary>
        public int day;

        /// <summary>The hour in the range 0..23.</summary>
        public int hour;

        /// <summary>The minute in the range 0..59.</summary>
        public int minute;

        /// <summary>The real-valued second in the half-open range [0, 60).</summary>
        public double second;

        CalendarDate(int year, int month, int day, int hour, int minute, double second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
    }
}