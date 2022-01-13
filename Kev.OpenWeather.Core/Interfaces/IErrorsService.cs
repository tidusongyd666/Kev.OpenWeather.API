using Kev.OpenWeather.Core.Errors;
using Kev.OpenWeather.Core.Errors.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Interfaces
{
    /// <summary>
    /// API errors builder
    /// </summary>
    public interface IErrorsService
    {
        /// <summary>
        /// Create error based on it's code
        /// </summary>
        ErrorDto CreateError(ErrorCode code, string modelErrors = null);
    }
}
