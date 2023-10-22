using System.Net;

namespace Duplocloud.API.Models
{
    /// <summary>
    /// Exception for invalid input, see app.UseExceptionHandler
    /// </summary>
    public class InvalidRequestException : Exception
    {
        /// <summary>
        /// Default HttpStatusCode.BadRequest Http status code
        /// </summary>
        public HttpStatusCode StatusCode = HttpStatusCode.BadRequest;

        /// <summary>
        /// Exception for invalid input
        /// </summary>
        /// <param name="Message">Custom reason for exception, returned in response</param>
        public InvalidRequestException(string Message = "Invalid Request Input")
            : base(Message)
        {
        }
    }
}
