using System.ComponentModel;

namespace Net.Infrastructure.BaseTypes.Models
{
    public class PagedRequest : IRequest
    {
        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1;

        [DefaultValue(100)]
        public int PageSize { get; set; } = 100;
    }
}
