using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http; 
using Varghese_Demo.API.DataValidation;
using Varghese_Demo.API.Models;

namespace Varghese_Demo.API.ProcessPayment.DataValidation
{
    internal class ProcessValidation : IProcessValidation
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IDataValidationControl dataValidation;
        public ProcessValidation(IDataValidationControl _dataValidation)
        {
            dataValidation = _dataValidation;
        } 

        public bool ValidateAuthentication(HttpRequestMessage paymentRequest, out HttpStatusCode statusCode, out string errorMessage)
        {
            return dataValidation.ValidateUserData(paymentRequest, out statusCode, out errorMessage);
        }

        public bool ValidateContentType(HttpRequestMessage paymentRequest, out HttpStatusCode statusCode, out string errorMessage)
        {
            errorMessage = string.Empty;
            statusCode = HttpStatusCode.OK;

            if ((paymentRequest == null || paymentRequest.Content == null || paymentRequest.Content.Headers.ContentType == null) || (!paymentRequest.Content.IsFormData() && paymentRequest.Content.Headers.ContentType.MediaType.ToLower().Trim() != "application/json"))
            {
                statusCode = paymentRequest != null || paymentRequest.Content == null || paymentRequest.Content.Headers.ContentType == null ? HttpStatusCode.NotAcceptable: HttpStatusCode.UnsupportedMediaType;
                errorMessage = paymentRequest.Content.IsMimeMultipartContent()? "Mime Multipart content is not supported!" : "The content type is not supported! (Supported content types are form and json (body))";
                logger.Error(errorMessage);
                return false;
            }

            return true;
        }

        public bool ValidatePaymentData(HttpRequestMessage paymentRequest, out HttpStatusCode statusCode, out string errorMessage, out PaymentData paymentData)
        {
            paymentData = new PaymentData();
            statusCode = HttpStatusCode.OK;

            try
            { 
                var dataDictionary = new Dictionary<string, string>();
                if (paymentRequest.Content.IsFormData())
                {
                    var paymentDta = paymentRequest.Content.ReadAsFormDataAsync().Result;
                    dataDictionary = paymentDta.AllKeys.Where(p => paymentDta[p] != null).ToDictionary(p => p, p => paymentDta[p]);
                }

                if (paymentRequest.Content.Headers.ContentType.MediaType.ToLower().Trim() == "application/json")
                {
                    var dta = paymentRequest.Content.ReadAsStringAsync().Result;
                    dataDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(dta);
                }

                if(!dataDictionary.Any())
                {
                    statusCode = HttpStatusCode.UnprocessableEntity;
                    errorMessage = "Process failed: Request data is missing ";
                    logger.Error(errorMessage + DateTime.Now);
                    return false;
                }
                    
                var isValidated = dataValidation.ValidatePaymentData(dataDictionary, out errorMessage, out paymentData); 
                if (!isValidated)
                {
                    statusCode = HttpStatusCode.UnprocessableEntity;
                    logger.Error(errorMessage + DateTime.Now);
                    return false;
                }
            }
            catch(Exception ex)
            {
                statusCode = HttpStatusCode.UnprocessableEntity;
                errorMessage = ex.ToString();
                logger.Error(errorMessage + DateTime.Now);
                return false;
            }

            return true;
        } 
    }
}
