using Prometheus; 

namespace Varghese_Demo.API.AppMetrics
{
    /// <summary>
    /// This feature should be improved. There is GUI to display the metrics
    /// </summary>
    public class ApplicationMetrics : IApplicationMetrics
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Histogram CreateHistogram()
        {
            return Metrics.CreateHistogram("dotnet_request_duration_seconds", "Histogram for the duration in seconds.",
                 new HistogramConfiguration
                 {
                     Buckets = Histogram.LinearBuckets(start: 1, width: 1, count: 5)
                 });
        }
    }
}
