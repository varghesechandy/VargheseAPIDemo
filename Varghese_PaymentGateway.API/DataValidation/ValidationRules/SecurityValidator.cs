using System.Linq;
using Varghese_PaymentGateway.API.Cryptography;

namespace Varghese_PaymentGateway.API.DataValidation.ValidationRules
{
    internal class SecurityValidator : IValidationRules
    { 

        public bool isMatch(string option)
        {
            return option == ValidationConsts.Instance.Security;
        }

        public void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage)
        {
            var dtaToValidate = dataToValidate;
            if (isEncrypted)
                dtaToValidate = cryptography.Decrypt(dataToValidate);

            errorMessage = string.Empty;
            if (!dtaToValidate.All(char.IsDigit) || dtaToValidate.Trim().Length != 3)
                errorMessage = "Security (CVV) is not valid! (3 digits)";
        }
    }
}
