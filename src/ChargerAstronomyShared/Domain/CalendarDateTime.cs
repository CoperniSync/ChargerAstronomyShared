using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain
{
    /// <summary>
    /// Represents a Gregorian calendar date and time within plus or minus 1 million years from the year 0.
    /// </summary>
    /// <remarks>
    /// The C# standard type `System.DateTime` only allows years from 0001 to 9999.
    /// However, the #AstroTime class can represent years in the range -999999 to +999999.
    /// In order to support formatting dates with extreme year values in an extrapolated
    /// Gregorian calendar, the `CalendarDateTime` class breaks out the components of
    /// a date into separate fields.
    /// </remarks>
    public struct CalendarDateTime
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

        public CalendarDateTime(int year, int month, int day, int hour, int minute, double second)
        {
            var local = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Local)
                            .AddSeconds(second);

            var utc = local.ToUniversalTime();

            this.year = utc.Year;
            this.month = utc.Month;
            this.day = utc.Day;
            this.hour = utc.Hour;
            this.minute = utc.Minute;
            this.second = utc.Second + utc.Millisecond / 1000.0;
        }


        /// <summary>Convert a J2000 day value to a Gregorian calendar date.</summary>
        /// <param name="ut">The real-valued number of days since the J2000 epoch.</param>
        public CalendarDateTime(double ut)
        {
            // Adapted from the NOVAS C 3.1 function cal_date().
            // Convert fractional days since J2000 into Gregorian calendar date/time.
            double djd = ut + 2451545.5;
            long jd = (long)Math.Floor(djd);
            double x = 24.0 * (djd % 1.0);
            if (x < 0.0)
                x += 24.0;
            hour = (int)x;
            x = 60.0 * (x % 1.0);
            minute = (int)x;
            second = 60.0 * (x % 1.0);

            // This is my own adjustment to the NOVAS cal_date logic
            // so that it can handle dates much farther back in the past.
            // I add c*400 years worth of days at the front,
            // then subtract c*400 years at the back,
            // which avoids negative values in the formulas that mess up
            // the calendar date calculations.
            // Any multiple of 400 years has the same number of days,
            // because it eliminates all the special cases for leap years.
            const long c = 2500;

            long k = jd + 68569 + c * 146097;
            long n = 4 * k / 146097;
            k = k - (146097 * n + 3) / 4;
            long m = 4000 * (k + 1) / 1461001;
            k = k - 1461 * m / 4 + 31;

            month = (int)(80 * k / 2447);
            day = (int)(k - 2447 * month / 80);
            k = month / 11;

            month = (int)(month + 2 - 12 * k);
            year = (int)(100 * (n - 49) + m + k - 400 * c);

            if (year < -999999 || year > +999999)
                throw new ArgumentOutOfRangeException("The supplied time is too far from the year 2000 to be represented.");

            if (month < 1 || month > 12 || day < 1 || day > 31)
                throw new InvalidOperationException($"Invalid calendar date calculated: month={month}, day={day}.");
        }

        /// <summary>
        /// Converts this `CalendarDateTime` to ISO 8601 format, expressed in UTC with millisecond resolution.
        /// </summary>
        /// <returns>Example: "2019-08-30T17:45:22.763Z".</returns>
        public override string ToString()
        {
            int millis = Math.Max(0, Math.Min(59999, (int)Math.Round(second * 1000.0)));
            string y;
            if (year < 0)
                y = "-" + (-year).ToString("000000");
            else if (year <= 9999)
                y = year.ToString("0000");
            else
                y = "+" + year.ToString("000000");
            return $"{y}-{month:00}-{day:00}T{hour:00}:{minute:00}:{millis / 1000:00}.{millis % 1000:000}Z";
        }
    }
}
