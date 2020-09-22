using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeDemo.API.AppDatabase.Entities;

namespace CodeDemo.API.AppDatabase.UsersRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        LoginUsers UserLogin(string username, string password, out string errorMsg);
    }
}
