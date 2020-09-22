using CodeDemo.API.Cryptography;

namespace CodeDemo.API.DataValidation.ValidationRules
{
    internal class AmountValidator : IValidationRules
    {
        public bool isMatch(string option)
        {
            return option.ToLower() == ValidationConsts.Instance.Amount.ToLower();
        }

        public void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage)
        {
            errorMessage = string.Empty;
            var dtaToValidate = decimal.TryParse(dataToValidate.ToString(), out decimal parsedData);

            if (!dtaToValidate)
            {
                errorMessage = "The field data for *Amount* is invalid!";
                return;
            }

            if(parsedData <= 0)
                errorMessage = "The amount should be greater than zero!"; 
        }
    }
}
