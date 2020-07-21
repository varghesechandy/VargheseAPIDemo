using System;
using System.Collections.Generic; 
using System.Linq;
using System.Net;
using System.Net.Http;
using Varghese_PaymentGateway.API.DataValidation.ValidationRules; 
using Varghese_PaymentGateway.API.Models; 
using Varghese_PaymentGateway.API.Cryptography;
using Varghese_PaymentGateway.API.AuthenticationService;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Varghese_PaymentGateway.API_TEST")]
namespace Varghese_PaymentGateway.API.DataValidation
{
    internal class DataValidationControl : IDataValidationControl
    { 
        private readonly List<IValidationRules> validationRule;
        private readonly IAuthService authService;  
        private readonly ICryptography cryptography;
      
        public DataValidationControl(ICryptography _cryptography, IAuthService _authService)
        {
            validationRule = new List<IValidationRules>
            {
                 new CreditCardNumberValidator(),
                 new ExpiryValidator(),
                 new NameValidator(),
                 new SecurityValidator(),
                 new AmountValidator(),
                 new CurrencyValidator()
            }; 
            cryptography = _cryptography;
            authService = _authService;
        } 

        public bool ValidatePaymentData(Dictionary<string,string> paymentDictionary, out string errorMessage, out PaymentData paymentData)
        { 
            return Validate(paymentDictionary, out errorMessage, out paymentData); 
        }

        private bool Validate(Dictionary<string,string> paymentDictionary, out string errorMessage, out PaymentData paymentData)
        {
            paymentData = new PaymentData();
            errorMessage = string.Empty;
            var ss = paymentDictionary[ValidationConsts.Instance.IsEncrypted];
            bool isEncrypted = paymentDictionary[ValidationConsts.Instance.IsEncrypted] != null && paymentDictionary[ValidationConsts.Instance.IsEncrypted] == "true";
            var paymentValidationChecker = ValidationConsts.Instance.DataChecker();
            foreach(var itemToValidate in paymentDictionary)
            {
                if (itemToValidate.Key == ValidationConsts.Instance.IsEncrypted) continue;
                validationRule.First(v => v.isMatch(itemToValidate.Key.ToLower())).Validate(itemToValidate.Value, isEncrypted, cryptography, out string errMsg);
                paymentValidationChecker[itemToValidate.Key.ToLower()] = true;
                if (errMsg.Trim() == string.Empty)
                {
                    CreatePaymentDataModel(ref paymentData, itemToValidate.Key.ToLower(), itemToValidate.Value, isEncrypted); 
                    continue;
                } 

                errorMessage += (errorMessage.Trim() == string.Empty ? errMsg : " / " + errMsg); 
            }

            var missingFields = paymentValidationChecker.Where(k => k.Value == false);
            if (!missingFields.Any() && errorMessage.Trim() == string.Empty)
                return true;

            var missingFieldErrors = errorMessage.Trim() == string.Empty ? "The required fields are missing;": "/ The required fields are missing;";
            foreach (var fields in missingFields)
                missingFieldErrors += "*" + fields.Key + "*";
            
            if (missingFields.Any())
                errorMessage += missingFieldErrors;

            return false;
        } 

        private void CreatePaymentDataModel(ref PaymentData paymentData, string fieldName, string value, bool isEncrypted)
        {
            if (fieldName == ValidationConsts.Instance.CardNumber)
                paymentData.CardNumber = isEncrypted ? cryptography.Decrypt(value) : value;  
            if (fieldName == ValidationConsts.Instance.Name) 
                paymentData.Name = value;  
            if (fieldName == ValidationConsts.Instance.Amount) 
                paymentData.Amount = Convert.ToDecimal(value);  
            if (fieldName == ValidationConsts.Instance.Expiry) 
                paymentData.Expiry = value;  
            if (fieldName == ValidationConsts.Instance.Currency) 
                paymentData.Currency = value;  
            if (fieldName == ValidationConsts.Instance.Security) 
                paymentData.Security = isEncrypted ? cryptography.Decrypt(value) : value;
        }

        public bool ValidateUserData(HttpRequestMessage request, out HttpStatusCode sCode, out string errorMessage)
        {  
            return authService.IsUserAuthenticatedForPaymentProcess(request, out sCode, out errorMessage); 
        } 
    }
}
