using System;
using System.Collections.Generic;

namespace AutomatedInvoiceGenerator.Helpers
{
    public static class DateTimeHelper
    {
        public static IEnumerable<DateTimePeriod> MonthsPeriodsBetweenDates(DateTimePeriod dateTimePeriod)
        {
            if (!(dateTimePeriod.StartDate.HasValue && dateTimePeriod.EndDate.HasValue))
                throw new Exception("Błąd wyznaczania miesięcznych okresów pomiędzy wskazanymi datami");

            DateTime runningDate = dateTimePeriod.StartDate.Value;
            while (runningDate < dateTimePeriod.EndDate)
            {
                DateTime nextMonthSeed = runningDate.AddMonths(1);
                DateTime toDate = CompareDates(new DateTime(nextMonthSeed.Year, nextMonthSeed.Month, 1), dateTimePeriod.EndDate.Value);
                DateTime periodStartDate = runningDate;
                DateTime periodEndDate = toDate.AddSeconds(-1);
                runningDate = toDate;
                yield return new DateTimePeriod(periodStartDate, periodEndDate);
            }
        }

        public static DateTime CompareDates(DateTime? dateFirst, DateTime? dateSecond)
        {
            if (!(dateFirst.HasValue && dateSecond.HasValue))
                throw new Exception("Błąd porównywania dat");

            return (dateFirst.Value < dateSecond.Value ? dateFirst.Value : dateSecond.Value);
        }

        public static DateTime CalculateLastSecondOfTheMonth(DateTime? date)
        {
            if (!date.HasValue)
                throw new Exception("Błąd wyznaczania ostatniej sekundy miesiąca");

            return new DateTime(date.Value.Year, date.Value.Month, 1).AddMonths(1).AddSeconds(-1);
        }

        public static decimal CalculatePeriodAsFractionOfMonth(DateTime startDate, DateTime endDate)
        {
            if (startDate.Year == endDate.Year && startDate.Month == endDate.Month && endDate > startDate)
            {
                DateTime firstSecondOfTheMonth = new DateTime(startDate.Year, startDate.Month, 1);
                DateTime lastSecondOfTheMonth = firstSecondOfTheMonth.AddMonths(1).AddSeconds(-1);

                return Math.Round((decimal)(((endDate - startDate).TotalSeconds) / (lastSecondOfTheMonth - firstSecondOfTheMonth).TotalSeconds),2);
            }

            throw new Exception("Błąd obliczania okresu czasu jako części miesiąca");
        }

        public class DateTimePeriod
        {
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public DateTimePeriod()
            {
                StartDate = null;
                EndDate = null;
            }

            public DateTimePeriod(DateTime? startDate, DateTime? endDate)
            {
                StartDate = startDate;
                EndDate = endDate;
            }

            public bool IsValid()
            {
                if (StartDate.HasValue && EndDate.HasValue)
                    if (EndDate.Value > StartDate.Value)
                        return true;

                return false;
            }
        }
    }
}
