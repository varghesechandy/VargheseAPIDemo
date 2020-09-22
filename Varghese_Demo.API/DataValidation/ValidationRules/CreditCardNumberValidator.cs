using System.Linq; 
using System.Text.RegularExpressions;
using CodeDemo.API.Cryptography;

namespace CodeDemo.API.DataValidation.ValidationRules
{
    internal class CreditCardNumberValidator : IValidationRules
    { 
        public bool isMatch(string option)
        {
            return option == ValidationConsts.Instance.CardNumber;
        }

        //This is not a proper data validation and this should be improved. If we use data annotaion, we can use its built in attribute
        //to validate a payment card number
        public void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage)
        {
            var dtaToValidate = dataToValidate;
            if (isEncrypted)
                dtaToValidate = cryptography.Decrypt(dataToValidate);

            errorMessage = string.Empty;
            //Replace all white space
            var dta = Regex.Replace(dtaToValidate, @"\s+", "");
            if (dta.Length > 16 || dta.Length < 13 || !dta.All(char.IsDigit) || dta.Trim()==string.Empty)
                errorMessage = "Credit card number is invalid!"; 
        }
    }
}
