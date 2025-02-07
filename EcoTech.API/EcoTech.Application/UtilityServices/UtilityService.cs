using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTech.Application.UtilityServices
{
    public class UtilityService : IUtilityService
    {
        public TimeSpan CalculateTimeUntil(double hoursFromNight12)
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today.AddHours(hoursFromNight12);
            if (now >= today)
            {
                today = today.AddDays(1);
            }
            return today - now;
        }
        public TimeSpan CalculateTimeUntilNextHours(double hoursFromNight12)
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today.AddHours(hoursFromNight12);
            if (now >= today)
            {
                today = today.AddDays(1);
            }
            return today - now;
        }


    }
}
