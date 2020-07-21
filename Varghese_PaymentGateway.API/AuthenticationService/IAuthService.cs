using System.Net;
using System.Net.Http; 
using Varghese_PaymentGateway.API.Models;

namespace Varghese_PaymentGateway.API.AuthenticationService
{
    /// <summary>
    /// Interface for Authentication Service
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// To generate a authentication token if successfully login
        /// </summary>
        User Authenticate(string username, string password, out string errorMessage);
        /// <summary>
        /// Validate a user request for payment process
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool IsUserAuthenticatedForPaymentProcess(HttpRequestMessage request, out HttpStatusCode statusCode, out string errorMessage);
    }
}
