using System.ComponentModel.DataAnnotations;

namespace Varghese_PaymentGateway.API.Models
{
    /// <summary>
    /// This information must be submitted to the payment gateway
    /// </summary>
    public class ProcessData
    {
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
        /// Option to submit the data either encrypted or plain
        /// </summary>
        public bool isencrypted { get; set; } = false;
    }
}
