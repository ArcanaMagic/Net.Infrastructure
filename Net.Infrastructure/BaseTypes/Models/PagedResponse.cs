namespace Net.Infrastructure.BaseTypes.Models
{
    public class PagedResponse<TResponse> : IResponse
    {
        public PageInfo<TResponse> ResponseInfo { get; set; }
    }
}