using Varghese_Demo.API.Cryptography;

namespace Varghese_Demo.API.DataValidation.ValidationRules
{
    internal interface IValidationRules
    {
        bool isMatch(string option);
        void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage);
    }
}
