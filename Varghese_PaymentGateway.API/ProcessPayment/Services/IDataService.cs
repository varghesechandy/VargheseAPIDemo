using System.Net.Http;
using Varghese_PaymentGateway.API.Models;

namespace Varghese_PaymentGateway.API.ProcessPayment.DataService
{
    internal interface IDataService
    {
        HttpResponseMessage SaveProcessData(PaymentData paymentData, string[] responseData); 
    }
}
