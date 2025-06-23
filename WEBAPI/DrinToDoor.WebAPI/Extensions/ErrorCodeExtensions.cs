using DrinkToDoor.BLL.Exceptions;
using System.Net;

namespace DrinToDoor.WebAPI.Extensions
{
    public static class ErrorCodeExtensions
    {
        private static readonly Dictionary<ErrorCode, HttpStatusCode> _metadata = new()
        {
            { ErrorCode.UNAUTHORIZED,          HttpStatusCode.Unauthorized },
            { ErrorCode.FORBIDDEN,             HttpStatusCode.Forbidden    },
            { ErrorCode.INTERNAL_SERVER_ERROR, HttpStatusCode.InternalServerError },
            { ErrorCode.LIST_EMPTY,            HttpStatusCode.BadRequest   },
            { ErrorCode.NOT_FOUND,             HttpStatusCode.NotFound     },
            { ErrorCode.HAS_INACTIVE,          HttpStatusCode.BadRequest   },
            { ErrorCode.HAS_EXISTED,           HttpStatusCode.BadRequest   },
            { ErrorCode.NOT_NULL,              HttpStatusCode.BadRequest   },
            { ErrorCode.INVALID,               HttpStatusCode.BadRequest   },

        };

        public static string GetMessage(this ErrorCode code, string customMessage = null)
        {
            if (!string.IsNullOrWhiteSpace(customMessage)
                && customMessage != code.ToString())
            {
                return customMessage;
            }
            return code.ToString();
        }
        public static HttpStatusCode GetStatusCode(this ErrorCode code)
           => _metadata.TryGetValue(code, out var sc)
              ? sc
              : HttpStatusCode.InternalServerError;
    }
}
