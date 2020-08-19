using Varghese_Demo.API.AppDatabase.Entities;
using Varghese_Demo.API.Models;
using Varghese_Demo.API.Pagination;

namespace Varghese_Demo.API.AppDatabase.PaymentProcessRepository
{
    /// <summary>
    /// Interface for a data repository
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// TO save payment data
        /// </summary>
        /// <param name="paymentProcessData"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool AddPaymentProcessData(PaymentProcess paymentProcessData, out string errorMessage);  
        /// <summary>
        /// To retreive details of a previously made payment
        /// </summary>
        /// <param name="resourceParameters"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        PagedList<PaymentProcess> GetPaymentHistory(ResourceParameters resourceParameters, out string errorMessage); 
    }
}
