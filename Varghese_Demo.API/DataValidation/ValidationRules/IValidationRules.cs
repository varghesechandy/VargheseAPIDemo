using CodeDemo.API.Cryptography;

namespace CodeDemo.API.DataValidation.ValidationRules
{
    internal interface IValidationRules
    {
        bool isMatch(string option);
        void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage);
    }
}
