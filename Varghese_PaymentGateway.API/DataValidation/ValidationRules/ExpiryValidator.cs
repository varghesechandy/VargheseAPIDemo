using System; 
using Varghese_Demo.API.Cryptography;

namespace Varghese_Demo.API.DataValidation.ValidationRules
{
    internal class ExpiryValidator : IValidationRules
    {
        public bool isMatch(string option)
        {
            return option == ValidationConsts.Instance.Expiry;
        }

        //I assume that the format of input data is mm/yyyy. This also needs to be improved
        public void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage)
        {
            errorMessage = string.Empty; 

            if (!dataToValidate.Contains("/"))
            {
                errorMessage = "The Expiry date is invalid! The format is 'mm/yyyy'";
                return;
            } 

            var month = Convert.ToInt32(dataToValidate.Split('/')[0]);
            var year = Convert.ToInt32(dataToValidate.Split('/')[1]);
            if ((year == DateTime.Now.Year && month <= DateTime.Now.Month) || year < DateTime.Now.Year)
                errorMessage = "The payment card is expired"; 
        }
    }
}
