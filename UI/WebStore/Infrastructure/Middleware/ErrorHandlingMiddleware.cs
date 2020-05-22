using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorHandlingMiddleware> _Logger;

        public ErrorHandlingMiddleware(RequestDelegate Next, ILogger<ErrorHandlingMiddleware> Logger)
        {
            _Next = Next;
            _Logger = Logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception e)
            {
                HandleException(context, e);
                throw;
            }
        }

        private void HandleException(HttpContext Context, Exception Error)
        {
            _Logger.LogError(Error, "Ошибка при обработке запроса {0}", Context.Request.Path);
        }
    }
}
