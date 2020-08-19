﻿using System.Net;
using System.Net.Http; 
using Varghese_Demo.API.Models;

namespace Varghese_Demo.API.ProcessPayment.DataValidation
{
    internal interface IProcessValidation
    {
        bool ValidateAuthentication(HttpRequestMessage paymentRequest, out HttpStatusCode statusCode, out string errorMessage);
        bool ValidateContentType(HttpRequestMessage paymentRequest, out HttpStatusCode statusCode, out string errorMessage);
        bool ValidatePaymentData(HttpRequestMessage paymentRequest, out HttpStatusCode statusCode, out string errorMessage, out PaymentData paymentData);
    }
}
