using System.Collections.Generic;
using System.Net;
using Net.Infrastructure.BaseTypes.Models;

namespace Net.Infrastructure.ErrorsHandling
{
    /// <summary>
    /// Класс ошибки который будет возвращен в видет модели респонза
    /// </summary>
    public class ErrorModel : IResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorType { get; set; }
        public IEnumerable<string> Lines { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}