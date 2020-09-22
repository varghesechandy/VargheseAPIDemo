using Microsoft.EntityFrameworkCore;
using System; 
using System.Linq;
using CodeDemo.API.AppDatabase.DatabaseContext;
using CodeDemo.API.AppDatabase.Entities;
using CodeDemo.API.Models;
using CodeDemo.API.Pagination;

namespace CodeDemo.API.AppDatabase.PaymentProcessRepository
{
    /// <summary>
    /// The actual implimentaion of database operations
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDataContext context;
        /// <summary>
        /// 
        /// </summary>
        public PaymentRepository(IRepositoryConnection dbConnection)
        {
            context = dbConnection.GetDataContext(); 
        }

        /// <summary>
        /// To save the details of the payment process 
        /// </summary>
        /// <param name="paymentProcessData"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool AddPaymentProcessData(PaymentProcess paymentProcessData, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            { 
                var entity = context.PaymentProcess.Add(paymentProcessData);
                entity.State = EntityState.Added;
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }

            return true;
        } 
        /// <summary>
        /// To retreive details of a previously made payment
        /// </summary>
        /// <param name="resourceParameters"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public PagedList<PaymentProcess> GetPaymentHistory(ResourceParameters resourceParameters, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var paymentHist = context.PaymentProcess.OrderByDescending(c=>c.PaymentDate) as IQueryable<PaymentProcess>;

                if(!string.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
                {
                    var searchQuery = resourceParameters.SearchQuery.Trim();
                    paymentHist = paymentHist.Where(a => a.PaymentId.Contains(searchQuery)
                                  || a.Name.Contains(searchQuery)
                                  || a.ResponseStatusCode.Contains(searchQuery));
                } 

                return PagedList<PaymentProcess>.Create(paymentHist, resourceParameters.PageNumber, resourceParameters.PageSize);                 
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return null;
            } 
        } 
    }
}
