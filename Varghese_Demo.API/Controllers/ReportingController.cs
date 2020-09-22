using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using CodeDemo.API.AppDatabase.PaymentProcessRepository;
using CodeDemo.API.Models;
using Prometheus;
using CodeDemo.API.AppMetrics;
using CodeDemo.API.Pagination;
using System;

namespace CodeDemo.API.Controllers
{
    /// <summary>
    /// Reporting
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController] 
    public class ReportingController : ControllerBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        readonly Counter counter = Metrics.CreateCounter("Reportingcounter", "Reporting counter"); 
        private readonly IPaymentRepository paymentRepository;
        private readonly IApplicationMetrics applicationMetrics;
        private readonly IMapper mapper; 

        /// <summary>
        /// This injects mapper and payament data repository and application metrics
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="_paymentRepository"></param>
        /// <param name="_applicationMetrics"></param>
        public ReportingController(IMapper _mapper, IPaymentRepository _paymentRepository, IApplicationMetrics _applicationMetrics)
        {
            if (_mapper != null)
                mapper = _mapper;

            paymentRepository = _paymentRepository;
            applicationMetrics = _applicationMetrics;
        }

        /// <summary>
        /// To retrieve details of a previously made payment.
        /// </summary>
        /// <returns>PaymentHistory</returns>
        [Consumes("application/json")]
        [HttpGet("PaymentHistory")]
        [HttpHead]
        public IActionResult PaymentHistory([FromQuery] ResourceParameters resourceParameters)
        {
            try
            {
                counter.Inc();
                var sw = System.Diagnostics.Stopwatch.StartNew();

                var payHistory = paymentRepository.GetPaymentHistory(resourceParameters, out string errorMessage);

                var previousPageLink = payHistory.HasPrevious ?
                                CreatePaymentHistoryResourceUri(resourceParameters, ResourceUriType.PreviousPage) : null;

                var nextPageLink = payHistory.HasNext ?
                                CreatePaymentHistoryResourceUri(resourceParameters, ResourceUriType.NextPage) : null;

                var paginationData = new
                {
                    totalCount = payHistory.TotalCount,
                    pageSize = payHistory.PageSize,
                    currentPage = payHistory.CurrentPage,
                    totalPages = payHistory.TotalPages,
                    previousPageLink,
                    nextPageLink
                };

                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationData));

                sw.Stop();
                applicationMetrics.CreateHistogram().Observe(sw.Elapsed.TotalSeconds);

                if (errorMessage.Trim() == string.Empty)
                    return Ok(mapper.Map<List<PaymentHistory>>(payHistory));

                logger.Error(errorMessage.Trim());
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return BadRequest();
        }


        private string CreatePaymentHistoryResourceUri(
           ResourceParameters resourceParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("PaymentHistory",
                      new
                      {
                          pageNumber = resourceParameters.PageNumber - 1,
                          pageSize = resourceParameters.PageSize, 
                          searchQuery = resourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("PaymentHistory",
                      new
                      {
                          pageNumber = resourceParameters.PageNumber + 1,
                          pageSize = resourceParameters.PageSize, 
                          searchQuery = resourceParameters.SearchQuery
                      });

                default:
                    return Url.Link("PaymentHistory",
                    new
                    {
                        pageNumber = resourceParameters.PageNumber,
                        pageSize = resourceParameters.PageSize, 
                        searchQuery = resourceParameters.SearchQuery
                    });
            }

        }

    }
}
