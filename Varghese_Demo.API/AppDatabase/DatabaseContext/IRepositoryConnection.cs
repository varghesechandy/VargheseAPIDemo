﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Varghese_Demo.API.AppDatabase.DatabaseContext
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRepositoryConnection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PaymentDataContext GetDataContext();
    }
}
