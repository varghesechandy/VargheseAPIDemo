using System;
using System.ComponentModel.DataAnnotations;

namespace CodeDemo.API.AppDatabase.Entities
{
    /// <summary>
    /// Database entity to save the payment process
    /// </summary>
    public class PaymentProcess
    {
        /// <summary>
        /// This must be generated from the aquiring bank
        /// </summary>
        [Key]
        public string PaymentId { get; set; }

        /// <summary>
        /// Payment card number
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Expiry date. The format should be 'MM/yyyy'
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Amount of payment
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Payment currency. This must be a string of 3 letters (GBP, USD, INR etc) 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Response status code from the Acquiring bank
        /// </summary>
        public string ResponseStatusCode { get; set; }

        /// <summary>
        /// Message from the Acquiring bank
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Date of payment
        /// </summary>
        public DateTime PaymentDate { get; set; }
    }
}
