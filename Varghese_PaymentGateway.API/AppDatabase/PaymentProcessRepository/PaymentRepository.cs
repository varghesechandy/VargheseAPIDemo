using Microsoft.EntityFrameworkCore;
using System; 
using System.Linq;
using Varghese_PaymentGateway.API.AppDatabase.DatabaseContext;
using Varghese_PaymentGateway.API.AppDatabase.Entities;
using Varghese_PaymentGateway.API.Models;
using Varghese_PaymentGateway.API.Pagination;

namespace Varghese_PaymentGateway.API.AppDatabase.PaymentProcessRepository
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

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public LoginUsers UserLogin(string username, string password, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            { 
                var usr = context.LoginUsers.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();                
                if(usr!=null)
                    return usr; 
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return null;
            }

            return null; ;
        }
    }
}
