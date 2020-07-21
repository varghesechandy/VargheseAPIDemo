using System.ComponentModel.DataAnnotations;

namespace Varghese_PaymentGateway.API.Models
{
    /// <summary>
    /// Send data to encrypt. An action called 'EncryptData' receives these information for encryption. 
    /// </summary>
    public class ProcessCardData
    {
        /// <summary>
        /// Payment card number 
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
        /// Paid Amount
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
    }
}
