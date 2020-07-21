using System.Runtime.CompilerServices;
using Xunit; 
using System.Collections.Generic; 
using Varghese_PaymentGateway.API.Models;
using Moq;
using Varghese_PaymentGateway.API.Cryptography;
using Varghese_PaymentGateway.API.AuthenticationService;
using Varghese_PaymentGateway.API.DataValidation;

[assembly: InternalsVisibleTo("Varghese_PaymentGateway.API_TEST")]
namespace Varghese_PaymentGateway.API_TEST
{
    public class DataValidationControlTest
    {  
        private Mock<ICryptography> cryptography = new Mock<ICryptography>(MockBehavior.Strict);
        private Mock<IAuthService> authService = new Mock<IAuthService>(MockBehavior.Strict);
        private readonly DataValidationControl dataValidationControl; 
        public DataValidationControlTest()
        {
            dataValidationControl = new DataValidationControl(cryptography.Object, authService.Object);
        }

        [Fact]
        public void ValidatePaymentData_ValidData()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_InValidCardNumber()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_InvalidCardNumber(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_EmptyCardnumber()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_EmptyCardNumber(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_InValidname()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_InvalidName(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_Emptyname()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_EmptyName(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_InvalidExpiry()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_Invalidexpirty(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_EmptyExpiry()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_Emptyexpirty(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_Expired()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_Expiredexpirty(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_Invalidamount()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_InvalidAmount(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_Invalidamount2()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_InvalidAmount2(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_Zeroamount()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_ZeroAmount(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_InvalidCurrency()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_Invalidcurrency(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_EmptyCurrency()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_Emptycurrency(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_Emptysecurity()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_EmptySecurity(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        [Fact]
        public void ValidatePaymentData_Invalidsecurity()
        {
            var res = dataValidationControl.ValidatePaymentData(ValidatePaymentData_InvalidSecurity(out bool result, out string errroMessage), out string errMsg, out PaymentData paymentData);
            Assert.Equal(result, res);
            Assert.Equal(errroMessage, errMsg);
        }

        private Dictionary<string,string> ValidatePaymentData(out bool result,out string errroMessage)
        {
            result = true;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567887654432" },
                { "name", "varghese" },
                { "expiry", "01/2030" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };
            errroMessage = string.Empty;
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_InvalidCardNumber(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567s87654432" },
                { "name", "varghese" },
                { "expiry", "01/2030" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "Credit card number is invalid!";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_EmptyCardNumber(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "" },
                { "name", "varghese" },
                { "expiry", "01/2030" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "Credit card number is invalid!";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_InvalidName(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "varghese123" },
                { "expiry", "01/2030" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The Name field must not have numbers!";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_EmptyName(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "" },
                { "expiry", "01/2030" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The Name field is required!";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_Invalidexpirty(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "test" },
                { "expiry", "012030" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The Expiry date is invalid! The format is 'mm/yyyy'";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_Emptyexpirty(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The Expiry date is invalid! The format is 'mm/yyyy'";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_Expiredexpirty(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2020" },
                { "amount", "144.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The payment card is expired";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_InvalidAmount(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2030" },
                { "amount", "14s.45" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The field data for *Amount* is invalid!";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_InvalidAmount2(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2030" },
                { "amount", "" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The field data for *Amount* is invalid!";
            return dictionary;
        }
        private Dictionary<string, string> ValidatePaymentData_ZeroAmount(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2030" },
                { "amount", "0" },
                { "currency", "GBP" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "The amount should be greater than zero!";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_Invalidcurrency(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2030" },
                { "amount", "123" },
                { "currency", "GBP2" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "Currency code is invalid";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_Emptycurrency(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2030" },
                { "amount", "122" },
                { "currency", "" },
                { "security", "005" },
                { "isencrypted", "false" }
            };

            errroMessage = "Currency code is invalid";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_EmptySecurity(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2030" },
                { "amount", "122" },
                { "currency", "GBP" },
                { "security", "" },
                { "isencrypted", "false" }
            };

            errroMessage = "Security (CVV) is not valid! (3 digits)";
            return dictionary;
        }

        private Dictionary<string, string> ValidatePaymentData_InvalidSecurity(out bool result, out string errroMessage)
        {
            result = false;
            var dictionary = new Dictionary<string, string>
            {
                { "cardnumber", "1234567487654432" },
                { "name", "Test" },
                { "expiry", "01/2030" },
                { "amount", "122" },
                { "currency", "GBP" },
                { "security", "005s" },
                { "isencrypted", "false" }
            };

            errroMessage = "Security (CVV) is not valid! (3 digits)";
            return dictionary;
        }
    }
}
