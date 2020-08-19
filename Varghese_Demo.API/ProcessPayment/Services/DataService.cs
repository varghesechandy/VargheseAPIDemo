using Microsoft.AspNetCore.Mvc; 
using System.Net.Http;
using System.Net; 
using Varghese_Demo.API.Models;
using Varghese_Demo.API.AppDatabase.PaymentProcessRepository;
using AutoMapper; 
using Varghese_Demo.API.AppDatabase.Entities; 

namespace Varghese_Demo.API.ProcessPayment.DataService
{ 
    internal class DataService : IDataService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IMapper mapper;
        public DataService(IPaymentRepository _paymentRepository, IMapper _mapper)
        {
            paymentRepository = _paymentRepository;
            if (_mapper != null)
                mapper = _mapper;
        }

        //Assemption: The acquiring bank returns a string array as a reponse of the payment request.
        //-------------------------------------------------------------------------------------------
        public HttpResponseMessage SaveProcessData(PaymentData paymentData, string[] responseData)
        {   
            paymentData.PaymentId = responseData != null ? responseData[0] : "N/A";
            paymentData.ResponseStatusCode = responseData != null ? responseData[1] : "20003";
            paymentData.Message = responseData != null ? responseData[2] : "Invalid Merchant or Merchant is not active";

            var paymentProcessData = mapper.Map<PaymentProcess>(paymentData);
            paymentRepository.AddPaymentProcessData(paymentProcessData, out string errorMessage);

            var errorResponse = new JsonResult(new { Id = paymentData.PaymentId, StatusCode =  paymentData.ResponseStatusCode, paymentData.Message });
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content =
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(errorResponse), System.Text.Encoding.UTF8,"application/json") 
            }; 
        }
    }
}
