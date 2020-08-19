using System;
namespace Varghese_Demo.API.Models
{
    /// <summary>
    /// To retrieve the details of a previously made payment
    /// </summary>
    public class PaymentHistory
    {
        /// <summary>
        /// Unique Id generated from the Acquiring bank
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Payment card number 
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Payment card holder name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Paid Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Response status code from the Acquiring bank
        /// </summary>
        public string ResponseStatusCode { get; set; }

        /// <summary>
        /// Message from the Acquiring bank
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Paid date
        /// </summary>
        public DateTime PaymentDate { get; set; }
    }
}
