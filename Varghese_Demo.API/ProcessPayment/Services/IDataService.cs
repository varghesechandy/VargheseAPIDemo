using System.Net.Http;
using CodeDemo.API.Models;

namespace CodeDemo.API.ProcessPayment.DataService
{
    internal interface IDataService
    {
        HttpResponseMessage SaveProcessData(PaymentData paymentData, string[] responseData); 
    }
}
