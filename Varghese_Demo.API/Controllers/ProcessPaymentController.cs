using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Prometheus;
using System;
using CodeDemo.API.Cryptography;
using CodeDemo.API.Models;

namespace CodeDemo.API.Controllers
{

    /// <summary>
    ///******************************************************************************************************************
    ///This is a mock controller for the API client. Swagger end point is not configured for the acquiring bank (Mock api)
    ///The production link is http://localhost/processpayment
    ///The action 'EncryptData' should be implemented in the client application
    ///******************************************************************************************************************
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController] 
    public class ProcessPaymentController : ControllerBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ICryptography cryptography;
        private readonly IMapper mapper;
        readonly Counter counter = Metrics.CreateCounter("PaymentCounter", "Payment counter");

        /// <summary>
        ///  
        /// </summary>
        public ProcessPaymentController(ICryptography _cryptography, IMapper _mapper)
        {
            cryptography = _cryptography;
            mapper = _mapper;
        }

        /// <summary>
        /// Please copy and paste the response data to 'PaymentSubmission' to encrypt the data before the payment process.
        /// </summary>
        [Consumes("application/json")]
        [HttpPost("EncryptData")]
        public IActionResult EncryptData([FromBody] ProcessCardData dta)
        {
            var dtaToMap = mapper.Map<ProcessData>(dta);
            try
            { 
                dtaToMap.CardNumber = cryptography.Encrypt(dta.CardNumber);
                dtaToMap.Security = cryptography.Encrypt(dta.Security);
                dtaToMap.isencrypted = true;
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(new { Message = "Encryption failed!" });
            }
            return Ok(dtaToMap);
        }

        /// <summary>
        /// There are two options to either send the data encrypted or unencryted to process a payment through the payment gateway.
        /// </summary>
        [Consumes("application/json")]
        [HttpPost("PaymentSubmission")]  
        public RedirectResult PaymentSubmission(ProcessData dta)
        {
            counter.Inc();
            return RedirectPreserveMethod("/processpayment");
        }
    }
}
