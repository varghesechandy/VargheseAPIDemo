using System.Linq; 
using Varghese_PaymentGateway.API.Cryptography;

namespace Varghese_PaymentGateway.API.DataValidation.ValidationRules
{
    internal class CurrencyValidator : IValidationRules
    {
        public bool isMatch(string option)
        {
            return option == ValidationConsts.Instance.Currency;
        }

        //This is not a proper currency code validation. This should be improved.
        public void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!dataToValidate.All(char.IsLetter) || dataToValidate.Trim().Length != 3)
                errorMessage = "Currency code is invalid";
        }
    }
}
