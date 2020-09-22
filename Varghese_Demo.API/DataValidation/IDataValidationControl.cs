using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using CodeDemo.API.Models;

namespace CodeDemo.API.DataValidation
{
    internal interface IDataValidationControl
    {
        bool ValidateUserData(HttpRequestMessage request, out HttpStatusCode sCode, out string errorMessage);
        bool ValidatePaymentData(Dictionary<string,string> paymentData, out string errorMessage, out PaymentData paymentModelData); 
    }
}
