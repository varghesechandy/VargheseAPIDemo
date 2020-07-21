using System.Collections.Generic; 

namespace Varghese_PaymentGateway.API.DataValidation
{
    /// <summary>
    /// Get all the validation constants
    /// </summary>
    public class ValidationConsts
    {
        private static ValidationConsts validationConsts = null;
        private ValidationConsts() { } 
        /// <summary>
        /// Global accessing point
        /// </summary>
        public static ValidationConsts Instance
        {
            get
            {
                if (validationConsts == null)
                    validationConsts = new ValidationConsts();

                return validationConsts;
            }
        } 
        /// <summary>
        /// CardNumber
        /// </summary>
        public string CardNumber => "cardnumber";
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "name";
        /// <summary>
        /// Expiry
        /// </summary>
        public string Expiry => "expiry";
        /// <summary>
        /// Amount
        /// </summary>
        public string Amount => "amount";
        /// <summary>
        /// Currency
        /// </summary>
        public string Currency => "currency";
        /// <summary>
        /// Security
        /// </summary>
        public string Security => "security";
        /// <summary>
        /// isEncrypted
        /// </summary>
        public string IsEncrypted => "isencrypted"; 
        /// <summary>
        /// To check all the required fields
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, bool> DataChecker()
        {
            var paymentFields = new Dictionary<string, bool>
                {
                    { CardNumber, false },
                    { Name, false },
                    { Expiry, false },
                    { Amount, false },
                    { Currency, false },
                    { Security, false }
                };

            return paymentFields;
        }
    }
}
