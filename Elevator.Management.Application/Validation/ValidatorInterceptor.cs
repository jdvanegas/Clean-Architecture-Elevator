using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Elevator.Management.Application.Validation
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeAspNetValidation(
            ActionContext actionContext, IValidationContext validationContext) =>
             validationContext;

        public ValidationResult AfterAspNetValidation(ActionContext controllerContext, IValidationContext validationContext,
            ValidationResult result)
        {
            if (result.Errors.Any())
                throw new Exceptions.ValidationException(result.Errors.Select(error => error.ErrorMessage));
            return result;
        }
    }
}