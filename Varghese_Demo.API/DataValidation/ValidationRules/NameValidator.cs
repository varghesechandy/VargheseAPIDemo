using System.Linq; 
using Varghese_Demo.API.Cryptography;

namespace Varghese_Demo.API.DataValidation.ValidationRules
{
    internal class NameValidator : IValidationRules
    {
        public bool isMatch(string option)
        {
            return option == ValidationConsts.Instance.Name;
        }

        public void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (dataToValidate.Any(char.IsDigit))
                errorMessage = "The Name field must not have numbers!";
            else if (dataToValidate.Trim() == string.Empty)
                errorMessage = "The Name field is required!"; 
        } 
    }
}
