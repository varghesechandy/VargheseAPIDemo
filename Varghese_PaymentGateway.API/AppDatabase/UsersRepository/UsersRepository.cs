using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Varghese_Demo.API.AppDatabase.DatabaseContext;
using Varghese_Demo.API.AppDatabase.Entities;

namespace Varghese_Demo.API.AppDatabase.UsersRepository
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly PaymentDataContext context;
        /// <summary>
        /// 
        /// </summary>
        public UsersRepository(IRepositoryConnection dbConnection)
        {
            context = dbConnection.GetDataContext();
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public LoginUsers UserLogin(string username, string password, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                var usr = context.LoginUsers.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
                if (usr != null)
                    return usr;
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return null;
            }

            return null; ;
        }
    }
}
