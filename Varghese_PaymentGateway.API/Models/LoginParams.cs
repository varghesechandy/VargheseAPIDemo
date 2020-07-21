namespace Varghese_PaymentGateway.API.Models
{
    /// <summary>
    /// To display appropriate fields for the login UI
    /// </summary>
    public class LoginParams
    {
        /// <summary>
        /// Username 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }
    }
}
