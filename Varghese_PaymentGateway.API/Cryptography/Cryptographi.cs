using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration; 
using Varghese_PaymentGateway.API.Models;

namespace Varghese_PaymentGateway.API.Cryptography
{
    /// <summary>
    /// To implement Encryption and Decryption
    /// </summary>
    public class Cryptographi : ICryptography
    { 
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// Create a data protector to encrypt and decrypt a string
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="_protectionProvider"></param>
        public Cryptographi(IConfiguration _configuration, IDataProtectionProvider _protectionProvider)
        { 
            var appSettingsSection = _configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            dataProtector = _protectionProvider.CreateProtector(appSettings.Key);
        }

        /// <summary>
        /// Decrypt a string
        /// </summary>
        /// <param name="itemToUnProtect"></param>
        /// <returns></returns>
        public string Decrypt(string itemToUnProtect)
        { 
            return dataProtector.Unprotect(itemToUnProtect);
        }

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="itemToProtect"></param>
        /// <returns></returns>
        public string Encrypt(string itemToProtect)
        { 
            return dataProtector.Protect(itemToProtect);
        }
    }
}
