using Prometheus;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CodeDemo.API.AppMetrics;
using CodeDemo.API.Models;
using CodeDemo.API.ProcessPayment.DataService;
using CodeDemo.API.ProcessPayment.DataValidation;

namespace CodeDemo.API.DelegatingHandlers
{
    /// <summary>
    /// The request to ocelot will be handled here before sending to acquiring bank
    /// </summary>
    internal class GatewayHandler : DelegatingHandler
    {  
        private readonly IProcessValidation processPaymentData;
        private readonly IDataService dataService;
        private readonly IApplicationMetrics applicationMetrics;
        private readonly Counter counter = Metrics.CreateCounter("OcelotHandler", "Payment Process");
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger(); 
        public GatewayHandler(IProcessValidation _processPaymentData, IDataService _dataService, IApplicationMetrics _applicationMetrics) 
        {
            logger.Info("Payment process started at " + DateTime.Now);
            processPaymentData = _processPaymentData;
            dataService = _dataService;
            applicationMetrics = _applicationMetrics;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            counter.Inc(); 
             var sw = System.Diagnostics.Stopwatch.StartNew();

            //Step 1: Authentication 
            if (!processPaymentData.ValidateAuthentication(request, out HttpStatusCode sCode, out string eMessage))
                return new HttpResponseMessage(sCode) { Content = new StringContent(eMessage) };
            
            //Step 2: Content Type Validation
            if (!processPaymentData.ValidateContentType(request, out HttpStatusCode statusCode, out string errorMessage))
                return new HttpResponseMessage(statusCode) { Content = new StringContent(errorMessage) };

            //Step 3: Data Validation 
            if (!processPaymentData.ValidatePaymentData(request, out HttpStatusCode responseCode, out string errMessage, out PaymentData paymentData))
                return new HttpResponseMessage(responseCode) { Content = new StringContent(errMessage) };

            //Step 4: Manage Response Message
            var response = await base.SendAsync(request, cancellationToken);
            HttpResponseMessage responseMsg = dataService.SaveProcessData(paymentData, response.IsSuccessStatusCode ? response.Content.ReadAsAsync<string[]>().Result : null);

            sw.Stop(); 
            applicationMetrics.CreateHistogram().Observe(sw.Elapsed.TotalSeconds);

            logger.Info("Payment process completed at " + DateTime.Now);
            return responseMsg;  
        } 
    }
}
