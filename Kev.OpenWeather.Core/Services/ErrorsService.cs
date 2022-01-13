using Kev.OpenWeather.Core.Errors;
using Kev.OpenWeather.Core.Errors.Models;
using Kev.OpenWeather.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Services
{
    public class ErrorsService : IErrorsService
    {
        public ErrorDto CreateError(ErrorCode code, string modelErrors = null)
        {
            var internalMessage = ApiErrorsInternal.ResourceManager.GetString("c" + (int)code);
            var userMessage = modelErrors ?? ApiErrors.ResourceManager.GetString("c" + (int)code);
            if (internalMessage == null || userMessage == null)
            {
                throw new InvalidOperationException($"User or internal error message with code 'c{(int)code} ({code})' is missed in the resources file");
            }
            return new ErrorDto
            {
                UserMessage = userMessage,
                InternalMessage = internalMessage,
                Code = (int)code,
            };
        }
    }
}
