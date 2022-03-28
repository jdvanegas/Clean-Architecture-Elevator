using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator.Management.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public List<string> ValdationErrors { get; set; }

        public ValidationException(IEnumerable<string> validationErrors)
        {
            ValdationErrors = new List<string>();

            foreach (var validationError in validationErrors.Distinct())
            {
                ValdationErrors.Add(validationError);
            }
        }
    }
}