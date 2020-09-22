using System.ComponentModel.DataAnnotations; 

namespace CodeDemo.API.Models
{
    /// <summary>
    /// All the information regarding the payment process including the request data and the response data 
    /// </summary>
    public class PaymentData 
    {
        /// <summary>
        /// This should be generated from the Aquired bank
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Payment card number. There is an option to encrypt this field
        /// </summary>
        [Required]
        public string CardNumber { get; set; }

        /// <summary>
        /// Payment card holder name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Expiry date. The format should be 'MM/yyyy'
        /// </summary>
        [Required]
        public string Expiry { get; set; }

        /// <summary>
        /// Amount to pay
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// Payment currency. This must be three letters long (GBP, USD, INR etc) 
        /// </summary>
        [Required]
        public string Currency { get; set; }

        /// <summary>
        /// Security code CVV
        /// </summary>
        [Required]
        public string Security { get; set; }

        /// <summary>
        /// Response status code from the Acquiring bank
        /// </summary>
        public string ResponseStatusCode { get; set; }

        /// <summary>
        /// Message from the Acquiring bank
        /// </summary>
        public string Message { get; set; }
    }
}
