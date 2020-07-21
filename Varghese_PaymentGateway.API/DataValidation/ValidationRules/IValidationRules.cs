﻿using Varghese_PaymentGateway.API.Cryptography;

namespace Varghese_PaymentGateway.API.DataValidation.ValidationRules
{
    internal interface IValidationRules
    {
        bool isMatch(string option);
        void Validate(string dataToValidate, bool isEncrypted, ICryptography cryptography, out string errorMessage);
    }
}
