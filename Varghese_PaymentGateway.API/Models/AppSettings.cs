namespace Varghese_PaymentGateway.API.Models
{
    /// <summary>
    /// To get the secret key
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// I am using this key for both authentication and Encryption
        /// </summary>
        public string Key { get; set; }
    }
}
