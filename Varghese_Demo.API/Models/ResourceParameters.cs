
namespace CodeDemo.API.Models
{
    /// <summary>
    /// This is for pagination
    /// </summary>
    public class ResourceParameters
    {
        const int maxPageSize = 20;
        /// <summary>
        /// 
        /// </summary>
        public string SearchQuery { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        private int _pageSize = 10;
        
        /// <summary>
        ///Page size cannot exceed more than 20. This is to improve the performance 
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
