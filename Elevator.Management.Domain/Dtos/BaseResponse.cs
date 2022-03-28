using System.Collections.Generic;

namespace Elevator.Management.Domain.Responses
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = true;
        }

        public BaseResponse(string message = null)
        {
            Success = true;
            Message = message;
        }

        public BaseResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }

        public BaseResponse(List<string> validationErrors)
        {
            Success = false;
            ValidationErrors = validationErrors;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}