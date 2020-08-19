using System.Net.Http;
using Varghese_Demo.API.Models;

namespace Varghese_Demo.API.ProcessPayment.DataService
{
    internal interface IDataService
    {
        HttpResponseMessage SaveProcessData(PaymentData paymentData, string[] responseData); 
    }
}
