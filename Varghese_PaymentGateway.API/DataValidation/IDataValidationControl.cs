using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Varghese_Demo.API.Models;

namespace Varghese_Demo.API.DataValidation
{
    internal interface IDataValidationControl
    {
        bool ValidateUserData(HttpRequestMessage request, out HttpStatusCode sCode, out string errorMessage);
        bool ValidatePaymentData(Dictionary<string,string> paymentData, out string errorMessage, out PaymentData paymentModelData); 
    }
}
