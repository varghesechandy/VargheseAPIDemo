﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeDemo.API.AppDatabase.DatabaseContext
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryConnection : IRepositoryConnection
    {  
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PaymentDataContext GetDataContext()
        {
           return new PaymentDataContext();
        }
    }
}
