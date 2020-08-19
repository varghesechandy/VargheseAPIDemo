using System.ComponentModel.DataAnnotations; 

namespace Varghese_Demo.API.AppDatabase.Entities
{
    /// <summary>
    /// Database entity for user information
    /// </summary>
    public class LoginUsers
    {
        /// <summary>
        /// Username
        /// </summary>
        [Key]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Role (Admin / User) Admin users are only allowed to do both payment submission and reporting
        /// </summary>
        public string Role { get; set; }
    }
}
