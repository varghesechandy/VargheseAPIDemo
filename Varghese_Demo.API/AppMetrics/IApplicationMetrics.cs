using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Varghese_Demo.API.AppMetrics
{
    /// <summary>
    /// This feature needs to be improved. There is no GUI option to see the performance
    /// </summary>
    public interface IApplicationMetrics
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Histogram CreateHistogram(); 
    }
}
