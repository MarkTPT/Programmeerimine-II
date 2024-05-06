using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Custom_Validation
{
    public class CustomScheduleDate : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (IUnitOfWork)validationContext
                         .GetService(typeof(IUnitOfWork));
            DateTime dateTime = Convert.ToDateTime(value).Date;
            var allDates = _context.Schedule.GetScheduleDates();
            DateTime dateNow = DateTime.Now.Date;
            var test = dateTime.CompareTo(dateNow) >= 0;
            if (!allDates.Contains(dateTime) && dateTime.CompareTo(dateNow) >= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult
                    ($"Please choose a date that hasn't been planned and later than {DateTime.Now.Date.ToShortDateString()}.");
            }
        }
    }
}
