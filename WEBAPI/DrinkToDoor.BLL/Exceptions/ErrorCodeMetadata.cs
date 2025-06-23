using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.Exceptions
{
    public class ErrorCodeMetadata
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }

        public ErrorCodeMetadata(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
